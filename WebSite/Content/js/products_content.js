
$(window).load(function() {


    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        Gallery("mobile");
        nowSection($('#products_contact_menu'));

    } else {
        Gallery("pc");
        nowSection($('#products_contact_menu'));
    }

    calcAccessories();
    specTab();
    
    
    PicZoom();
    AccessoriesSlider();

    if ($(window).width() <= 450) {

        sectionBtn();

    } else {

        ctrl_pd_follow();

    }
})


function Gallery(Device) {
    if (Device == "pc") {
        $('.album').slick({
            nextArrow: "<button type='button' class='slick-next'><img src='/content/images/gallery_arrow_03.png'/></button>",
            prevArrow: "<button type='button' class='slick-prev'><img src='/content/images/gallery_arrow_03.png'/></button>",
            dots: true,
            autoplay: true

        });


    } else if (Device == "mobile") {

        $('.album').slick({
            nextArrow: "<button type='button' class='slick-next'><img src='/content/images/gallery_arrow_03.png'/></button>",
            prevArrow: "<button type='button' class='slick-prev'><img src='/content/images/gallery_arrow_03.png'/></button>",
            dots: true,
            autoplay: true,
            infinite: true,
            responsive: [{
                breakpoint: 430,
                settings: {
                    arrows: false
                }
            }]
        });
    }
}

function calcAccessories() {
    var titleHeightSize = $('.container .name').size();
    var HeightArray = [];
    var titleHeight
    var MaxHeight
    for (var i = 0; i < titleHeightSize; i++) {
        titleHeight = titleHeight = $('.container .name').eq(i).height();
        HeightArray.push(titleHeight);
    }
    MaxHeight = HeightArray.max();
    PaddingValue = parseInt($('.container .name').css("padding-top"));
    $('.container .name').css({ 'height': MaxHeight + (PaddingValue * 2) + 'px' });



    // console.log(PaddingValue);
}

function sectionBtn() {
    $('.mainContent').slideUp();
    $('.mobile_section_btn.active').next().slideDown();
    $('.mobile_section_btn').click(function() {
        $(this).next().slideToggle();
        //$('.mobile_section_btn').toggleClass('active');
        $(this).toggleClass('active');

    })

}
// -------------------------------------控制規格的切換
function specTab() {
    var nowTab
    $('table.sep_table').not('.active').fadeOut();
    $('.spc_tab a').click(
        function() {
            $('.spc_tab a').removeClass('active');
            $(this).addClass('active');
            nowTab = $(this).index();
            // console.log(nowTab);
            $('.spe_main .sep_table').removeClass('active');
            $('.spe_main #sep_' + nowTab).addClass('active');
            $('.spe_main .sep_table').not('.active').fadeOut(0);
            $('.spe_main table.active').fadeIn();


        })

}
// -------------------------------------控制規格的切換
// -------------------------------------控制規格的切換
function ctrl_pd_follow() {
    var windowHeight = $(window).height();
    var obj_p = $('.products_contact_menu.static').offset();
    var menuHeight = $('.products_contact_menu').height() + obj_p.top;

    $(window).scroll(function() {

        window_scrollTop = $(window).scrollTop();


        if (window_scrollTop >= menuHeight) {
            //console.log(menuHeight);

            $('.products_contact_menu.fix').addClass("open");
            $('.mainmenu').addClass("hide");
            //$('.secMenu').addClass("sec");



        } else {

            $('.products_contact_menu.fix').removeClass("open");
            $('.mainmenu').removeClass("hide");
            //$('.secMenu').removeClass("sec");

        }

    })

}
// -------------------------------------控制規格的切換
// -------------------------------------點擊大圖
function PicZoom() {
    $('#containerLargePic').zoom();
}
// -------------------------------------點擊大圖
// -------------------------------------配件slider
function AccessoriesSlider() {


    $(document).ready(function() {
        var owl = $('.owl-carousel');
        owl.owlCarousel({
            nav: false,
            items: 1,
            margin: 30,
            URLhashListener:true,
            autoplayHoverPause:true,
            smartSpeed:0,
            startPosition: 'URLHash'
            //rtl:true,
        });
        owl.on('changed.owl.carousel', function(event) {

            var page = event.page.index;
            if(page==0){
                $('#AccessoriesOpen .prevBtn').addClass('disable');
                $('#AccessoriesOpen .nextBtn').removeClass('disable');
            }else if(page == $('.owl-item').size() - 1){
                 $('#AccessoriesOpen .nextBtn').addClass('disable');
                 $('#AccessoriesOpen .prevBtn').removeClass('disable');
            }else{

                $('#AccessoriesOpen .nextBtn').removeClass('disable');
                $('#AccessoriesOpen .prevBtn').removeClass('disable');


            }
            // console.log(page);

        })
        $('#AccessoriesOpen .nextBtn').click(function() {
            owl.trigger('next.owl.carousel',[300]);
        })
        $('#AccessoriesOpen .prevBtn').click(function() {
            owl.trigger('prev.owl.carousel',[300]);
        })

    });


}
// -------------------------------------配件slider
//-------------------------------判斷規格的tab數量

$(window).load(function(){

    console.log($('.spc_tab a').size());
    if($('.spc_tab a').size()==1){
        $('.spc_tab a').removeClass("small-6");
        $('.spc_tab a').addClass("small-12");
    }
})
//-------------------------------判斷規格的tab數量

//-------------------------------判斷配件章節數量
$(window).load(function(){
    var sectionQuantity=$('section').size();
    console.log("sectionQuantity="+sectionQuantity);
    $('section').eq(sectionQuantity-2).addClass('lastSection');
})

//-------------------------------判斷配件章節數量
