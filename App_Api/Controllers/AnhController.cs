using App_Api.Storage;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.Anh;
using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnhController : ControllerBase
{
    private const string ProductImageFolder = "AnhSanPham";

    private readonly IAllRepo<Anh> _imageRepository;
    private readonly IImageStorage _imageStorage;
    private readonly ILogger<AnhController> _logger;

    public AnhController(
        IAllRepo<Anh> imageRepository,
        IImageStorage imageStorage,
        ILogger<AnhController> logger)
    {
        _imageRepository = imageRepository;
        _imageStorage = imageStorage;
        _logger = logger;
    }

    [HttpPost("create-list-image")]
    [RequestSizeLimit(30 * 1024 * 1024)]
    public async Task<IActionResult> CreateImage(
        [FromForm] string idProductDetail,
        [FromForm] List<IFormFile> lstIFormFile,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(idProductDetail) || lstIFormFile.Count == 0)
        {
            return BadRequest("Thiếu mã sản phẩm hoặc danh sách ảnh.");
        }

        var savedFiles = new List<string>();
        try
        {
            foreach (var file in lstIFormFile)
            {
                var fileName = await _imageStorage.SaveAsync(
                    file,
                    ProductImageFolder,
                    cancellationToken: cancellationToken);

                var saved = _imageRepository.AddItem(new Anh
                {
                    IdAnh = Guid.NewGuid().ToString(),
                    IdSanPhamChiTiet = idProductDetail,
                    NgayTao = DateTime.UtcNow,
                    Url = fileName,
                    TrangThai = 0
                });

                if (!saved)
                {
                    _imageStorage.Delete(ProductImageFolder, fileName);
                    throw new InvalidOperationException("Không thể lưu thông tin ảnh vào database.");
                }

                savedFiles.Add(fileName);
            }

            return Ok(new { files = savedFiles });
        }
        catch (InvalidDataException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Không thể lưu ảnh cho sản phẩm {ProductDetailId}.", idProductDetail);
            return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lưu ảnh sản phẩm.");
        }
    }

    [HttpPost("upload-anh")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> UploadImages(
        [FromForm] List<IFormFile> files,
        CancellationToken cancellationToken)
    {
        if (files.Count == 0)
        {
            return BadRequest("Chưa chọn ảnh để tải lên.");
        }

        try
        {
            var savedFiles = new List<string>();
            foreach (var file in files)
            {
                savedFiles.Add(await _imageStorage.SaveAsync(
                    file,
                    ProductImageFolder,
                    preserveFileName: true,
                    cancellationToken));
            }

            return Ok(new { files = savedFiles });
        }
        catch (InvalidDataException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (IOException exception)
        {
            return Conflict(exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Không thể tải danh sách ảnh sản phẩm.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Không thể tải ảnh lên.");
        }
    }

    [HttpDelete("delete-list-image")]
    public IActionResult DeleteListImage([FromBody] ImageDTO request)
    {
        if (request.lstImageRemove is null || request.lstImageRemove.Count == 0)
        {
            return BadRequest("Danh sách ảnh cần xóa đang trống.");
        }

        var images = _imageRepository.GetAll()
            .Where(image => image.IdSanPhamChiTiet == request.idProductDetail
                && request.lstImageRemove.Contains(image.Url!))
            .ToList();

        foreach (var image in images)
        {
            image.TrangThai = 1;
            _imageRepository.EditItem(image);
            if (!string.IsNullOrWhiteSpace(image.Url))
            {
                _imageStorage.Delete(ProductImageFolder, image.Url);
            }
        }

        return Ok(new { deleted = images.Count });
    }

    [HttpPost("create-list-model-image")]
    public IActionResult CreateModelNameImage(
        [FromForm] string idProductDetail,
        [FromForm] List<string> lstNameImage)
    {
        if (string.IsNullOrWhiteSpace(idProductDetail) || lstNameImage.Count == 0)
        {
            return BadRequest("Thiếu mã sản phẩm hoặc tên ảnh.");
        }

        foreach (var name in lstNameImage.Select(Path.GetFileName))
        {
            var fullPath = Path.Combine(_imageStorage.GetDirectory(ProductImageFolder), name);
            if (!System.IO.File.Exists(fullPath))
            {
                return BadRequest($"Ảnh '{name}' chưa được tải lên.");
            }

            _imageRepository.AddItem(new Anh
            {
                IdAnh = Guid.NewGuid().ToString(),
                IdSanPhamChiTiet = idProductDetail,
                NgayTao = DateTime.UtcNow,
                TrangThai = 0,
                Url = name
            });
        }

        return Ok();
    }
}
