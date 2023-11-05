@page "/DonHangThanhCong"
@{
    Layout = "/Views/Shared/_LayoutUser.cshtml";
}

<div class="col-lg-9 col-md-12">
    <div class="dash__box dash__box--shadow dash__box--radius dash__box--bg-white u-s-m-b-30">
        <div class="dash__pad-2">
            <h1 class="dash__h1 u-s-m-b-14">My Orders</h1>

            <span class="dash__text u-s-m-b-30">Here you can see all products that have been delivered.</span>
            <form class="m-order u-s-m-b-30">
                <div class="m-order__select-wrapper">

                    <label class="u-s-m-r-8" for="my-order-sort">Show:</label><select class="select-box select-box--primary-style" id="my-order-sort">
                        <option selected>Last 5 orders</option>
                        <option>Last 15 days</option>
                        <option>Last 30 days</option>
                        <option>Last 6 months</option>
                        <option>Orders placed in 2018</option>
                        <option>All Orders</option>
                    </select>
                </div>
            </form>
            <div class="m-order__list">
                <div class="m-order__get">
                    <div class="manage-o__header u-s-m-b-30">
                        <div class="dash-l-r">
                            <div>
                                <div class="manage-o__text-2 u-c-secondary">Order #305423126</div>
                                <div class="manage-o__text u-c-silver">Placed on 26 Oct 2016 09:08:37</div>
                            </div>
                            <div>
                                <div class="dash__link dash__link--brand">

                                    <a href="dash-manage-order.html">MANAGE</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="manage-o__description">
                        <div class="description__container">
                            <div class="description__img-wrap">

                                <img class="u-img-fluid" src="images/product/electronic/product3.jpg" alt="">
                            </div>
                            <div class="description-title">Yellow Wireless Headphone</div>
                        </div>
                        <div class="description__info-wrap">
                            <div>

                                <span class="manage-o__badge badge--processing">Processing</span>
                            </div>
                            <div>

                                <span class="manage-o__text-2 u-c-silver">
                                    Quantity:

                                    <span class="manage-o__text-2 u-c-secondary">1</span>
                                </span>
                            </div>
                            <div>

                                <span class="manage-o__text-2 u-c-silver">
                                    Total:

                                    <span class="manage-o__text-2 u-c-secondary">$16.00</span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="m-order__get">
                    <div class="manage-o__header u-s-m-b-30">
                        <div class="dash-l-r">
                            <div>
                                <div class="manage-o__text-2 u-c-secondary">Order #305423126</div>
                                <div class="manage-o__text u-c-silver">Placed on 26 Oct 2016 09:08:37</div>
                            </div>
                            <div>
                                <div class="dash__link dash__link--brand">

                                    <a href="dash-manage-order.html">MANAGE</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="manage-o__description">
                        <div class="description__container">
                            <div class="description__img-wrap">

                                <img class="u-img-fluid" src="images/product/women/product8.jpg" alt="">
                            </div>
                            <div class="description-title">New Dress D Nice Elegant</div>
                        </div>
                        <div class="description__info-wrap">
                            <div>

                                <span class="manage-o__badge badge--shipped">Shipped</span>
                            </div>
                            <div>

                                <span class="manage-o__text-2 u-c-silver">
                                    Quantity:

                                    <span class="manage-o__text-2 u-c-secondary">1</span>
                                </span>
                            </div>
                            <div>

                                <span class="manage-o__text-2 u-c-silver">
                                    Total:

                                    <span class="manage-o__text-2 u-c-secondary">$16.00</span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="m-order__get">
                    <div class="manage-o__header u-s-m-b-30">
                        <div class="dash-l-r">
                            <div>
                                <div class="manage-o__text-2 u-c-secondary">Order #305423126</div>
                                <div class="manage-o__text u-c-silver">Placed on 26 Oct 2016 09:08:37</div>
                            </div>
                            <div>
                                <div class="dash__link dash__link--brand">

                                    <a href="dash-manage-order.html">MANAGE</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="manage-o__description">
                        <div class="description__container">
                            <div class="description__img-wrap">

                                <img class="u-img-fluid" src="images/product/men/product8.jpg" alt="">
                            </div>
                            <div class="description-title">New Fashion D Nice Elegant</div>
                        </div>
                        <div class="description__info-wrap">
                            <div>

                                <span class="manage-o__badge badge--delivered">Delivered</span>
                            </div>
                            <div>

                                <span class="manage-o__text-2 u-c-silver">
                                    Quantity:

                                    <span class="manage-o__text-2 u-c-secondary">1</span>
                                </span>
                            </div>
                            <div>

                                <span class="manage-o__text-2 u-c-silver">
                                    Total:

                                    <span class="manage-o__text-2 u-c-secondary">$16.00</span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>