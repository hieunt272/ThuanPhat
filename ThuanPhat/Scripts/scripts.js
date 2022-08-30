AOS.init();

function homeJs() {
    $(".marquee").marquee({
        duration: 5000,
        duplicated: true,
        gap: 00,
        direction: "up",
        pauseOnHover: true
    });

    $(".banner-list").slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: true,
        autoplay: true,
        autoplaySpeed: 3000,
        dots: true,
        prevArrow: $(".banner .prev"),
        nextArrow: $(".banner .next"),
        appendDots: $(".banner .dots")
    });

    $(".restart").click(function () {
        $(this).addClass("hidden");
        $(".stop").addClass("active");
        $(".banner-list").slick("slickPause");
    });

    $(".stop").click(function () {
        $(this).removeClass("active");
        $(".restart").removeClass("hidden");
        $(".banner-list").slick("slickPlay");
    });

    var mw1360 = window.matchMedia("(max-width: 1360px)");
    var mw768 = window.matchMedia("(min-width: 768px)");
    if (mw768.matches && mw1360.matches) {
        $(".service .row").slick({
            dots: false,
            infinite: true,
            speed: 1000,
            slidesToShow: 2,
            slidesToScroll: 1,
            autoplay: true,
            speed: 1500,
            autoplaySpeed: 3000,
            prevArrow: '<button class="chevron-prev"><i class="fas fa-chevron-left"></i></button>',
            nextArrow: '<button class="chevron-next"><i class="fas fa-chevron-right"></i></button>',
        });
    }

    $(".training-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="fas fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="fas fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
}

function alertBox() {
    $("#AlertBox").fadeOut(1000);
}

$(function () {
    $(".recent-post-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="fas fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="fas fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 470,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });

    $(".contact-form").on("submit", function (e) {
        e.preventDefault();
        $.post("/Home/ContactForm", $(this).serialize(), function (data) {
            if (data.status) {
                $.toast({
                    heading: "Contact successfully sent",
                    text: data.msg,
                    icon: "success"
                });
                $(".contact-form").trigger("reset");
            } else {
                $.toast({
                    heading: "Failed to send",
                    text: data.msg,
                    icon: "error"
                });
            }
        });
    });

    $(".subcribe-form").on("submit", function (e) {
        e.preventDefault();
        $.post("/Home/SubcribeForm", $(this).serialize(), function (data) {
            if (data.status) {
                $.toast({
                    heading: "Sign up to receive news successfully",
                    text: data.msg,
                    icon: "success"
                });
                $(".subcribe-form").trigger("reset");
            } else {
                $.toast({
                    heading: "Subscription failed",
                    text: data.msg,
                    icon: "error"
                });
            }
        });
    });

    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $(".header").addClass("active");
            $(".header-mb").addClass("active");
        } else {
            $(".header").removeClass("active");
            $(".header-mb").removeClass("active");
        }
        if ($(this).scrollTop() > 200) {
            $(".btn-scroll").fadeIn(200);
        } else {
            $(".btn-scroll").fadeOut(200);
        }
    });

    $(".btn-scroll").click(function (event) {
        event.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, 300);
    });

    $(".btn-search").click(function () {
        $(".header-search").toggleClass("active");
    });

    $(".hamburger").click(function () {
        $(this).toggleClass("active");
        $(".menu-mb").toggleClass("active");
        $(".overlay").toggleClass("active");
    });

    $(".overlay").click(function () {
        $(this).removeClass("active");
        $(".menu-mb").removeClass("active");
        $(".hamburger").removeClass("active");
    });

    setTimeout(alertBox, 3000);

    if (window.location.pathname == "/ja") {
        $(".languages .ja").addClass("active");
        $(".languages .vn").removeClass("active");
    }
});