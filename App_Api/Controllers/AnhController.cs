using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.Anh;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnhController : ControllerBase
    {
        private readonly IAllRepo<Anh> _allRepoImage;

        public AnhController(IAllRepo<Anh> allRepoImage)
        {
            _allRepoImage = allRepoImage;
        }
        [HttpPost("create-list-image")]
        public async Task<IActionResult> CreateImage([FromForm] string idProductDetail, [FromForm] List<IFormFile> lstIFormFile)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string rootPath = Directory.GetParent(currentDirectory)!.FullName;
                string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "AnhSanPham");

                foreach (var file in lstIFormFile)
                {
                    if (file.Length > 0)
                    {
                        using var stream = new MemoryStream();
                        file.CopyTo(stream);
                        stream.Position = 0;

                        using var image = SixLabors.ImageSharp.Image.Load(stream);

                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new SixLabors.ImageSharp.Size(1600, 1600),
                            Mode = ResizeMode.Max
                        }));

                        var encoder = new JpegEncoder
                        {
                            Quality = 80
                        };

                        string fileName = Guid.NewGuid().ToString() + file.FileName;
                        string outputPath = Path.Combine(uploadDirectory, fileName);

                        using var outputStream = new FileStream(outputPath, FileMode.Create);
                        await image.SaveAsync(outputStream, encoder);
                        _allRepoImage.AddItem(new Anh { IdAnh = Guid.NewGuid().ToString(), IdSanPhamChiTiet = idProductDetail, NgayTao = DateTime.Now, Url = fileName, TrangThai = 0 });
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-anh")]
        public async Task<bool> CreateImage([FromForm]List<IFormFile> files)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string rootPath = Directory.GetParent(currentDirectory)!.FullName;
                string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "AnhSanPham");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using var stream = new MemoryStream();
                        file.CopyTo(stream);
                        stream.Position = 0;

                        using var image = SixLabors.ImageSharp.Image.Load(stream);

                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new SixLabors.ImageSharp.Size(1600, 1600),
                            Mode = ResizeMode.Max
                        }));

                        var encoder = new JpegEncoder
                        {
                            Quality = 80
                        };

                        string fileName = file.FileName;
                        string outputPath = Path.Combine(uploadDirectory, fileName);

                        using var outputStream = new FileStream(outputPath, FileMode.Create);
                        await image.SaveAsync(outputStream, encoder);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        [HttpDelete("delete-list-image")]
        public IActionResult DeleteListImage(ImageDTO responseImageDeleteVM)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string rootPath = Directory.GetParent(currentDirectory)!.FullName;
            string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "AnhSanPham");
            try
            {
                foreach (var item in _allRepoImage.GetAll().Where(im => im.IdSanPhamChiTiet == responseImageDeleteVM.idProductDetail && responseImageDeleteVM.lstImageRemove!.Contains(im.Url!)))
                {
                    item.TrangThai = 1;
                    _allRepoImage.EditItem(item);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("create-list-model-image")]
        public bool CreateModelNameImage([FromForm]string idProductDetail,[FromForm]List<string> lstNameImage)
        {
            try
            {
                foreach (var item in lstNameImage)
                {
                    var modelAnh = new Anh()
                    {
                        IdAnh = Guid.NewGuid().ToString(),
                        IdSanPhamChiTiet = idProductDetail,
                        NgayTao = DateTime.Now,
                        TrangThai = 0,
                        Url = item
                    };
                    _allRepoImage.AddItem(modelAnh);
                };
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

    }
}
