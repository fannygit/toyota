$(window).load(function() {
    // Run code
    //---------------------------------------------------------------imagesLoaded
    $('body').imagesLoaded()
        .always(function(instance) {
            console.log('all images loaded');

        })
        .done(function(instance) {
            console.log('all images successfully loaded');
            var tween_loader = TweenMax.to($('.loader_container'), 0.6, {
                opacity: .0,
                onComplete: loadComplete
            })
        })
        .fail(function() {
            console.log('all images loaded, at least one is broken');
        })
        .progress(function(instance, image) {
            var result = image.isLoaded ? 'loaded' : 'broken';
            console.log('image is ' + result + ' for ' + image.img.src);
        });
    //---------------------------------------------------------------imagesLoaded    


    // ----------------------------------------Call the rippler
    $(".rippler").rippler({
        effectClass: 'rippler-effect',
        effectSize: 12 // Default size (width & height)
            ,
        addElement: 'div' // e.g. 'svg'(feature)
            ,
        duration: 500
    });
    // ----------------------------------------Call the rippler

    // if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    //     crt_clickMenu();
    //     ctrl_footer();
    //     ctrl_menu_mobile();
    // } else {
    //     // ----------------------------------------首頁youtube API 控制  

    //     // ----------------------------------------首頁youtube API 控制
    //     // ----------------------------------------產品列表 控制  
    //     ctrl_menu_pc();
    //     ctrl_menu_follow();

    //     // ----------------------------------------產品列表 API 控制  
    // }
     if ($(window).width() <= 1024) {
        crt_clickMenu();
        ctrl_footer();
        ctrl_menu_mobile();
    } else {
        // ----------------------------------------首頁youtube API 控制  

        // ----------------------------------------首頁youtube API 控制
        // ----------------------------------------產品列表 控制  
        ctrl_menu_pc();
        ctrl_menu_follow();
        searchCtrl();

        // ----------------------------------------產品列表 API 控制  
    }
});

var loadComplete = function() {
    $('.loader_container').css({ "display": "none", "left": 100 + "%" });

}

//-----------------------------------------------------控制輸入框


function resettext(id) {
    //恢復文字
    if (id.value == "") {
        id.value = id.defaultValue;
        $(id).removeClass("onType");

    }
}

function cleartext(id) {
    //清除文字
    id.value = "";
    $(id).addClass("onType");

}
//-----------------------------------------------------控制輸入框

//控制-------------------------------------mobile選單
function crt_clickMenu() {
    // /$('.sub_car_gallery').slideUp(1000);
    $('.click_menu').slideUp(0);
    $('.products_list').slideUp(0);
    $('.sub_menu_container').slideUp(0);
    $('.clickArea').css({ 'height': 73 + 'px' });
    $('.clickArea').click(function() {
        if ($(this).parent().hasClass('sub')) {
            $(this).parent().toggleClass('open');
            $(this).prev('img').toggleClass('open');
            $(this).next('.sub_menu_container').slideToggle(400);
        } else {}
    })
    $('.category_title').click(function() {
        $(this).next('.products_list').slideToggle(500);
        $(this).find('img').toggleClass('open');
    })
    $('button.hamburger').click(function() {
        $(this).toggleClass('is-active');
        $('.click_menu').slideToggle(500);
    })

    $('.list_cover').click(function() {
        console.log('haha');
    })
}
//控制-------------------------------------mobile選單
//控制-------------------------------------mobile footer

function ctrl_footer() {
    $('.container .unit').find('.items').slideUp(0);
    $('.container .unit').click(function() {
        $('.container .unit .hide_arrow').removeClass('Selected');
        $('.container .unit .items').removeClass('Selected');
        $('.container .unit .title_container').removeClass('Selected');

        $(this).find('.hide_arrow').addClass('open');
        $(this).find('.hide_arrow').addClass('Selected');
        $(this).find('.items').slideDown(300);
        $(this).find('.items').addClass('Selected');
        $(this).find('.title_container').addClass('open');
        $(this).find('.title_container').addClass('Selected');

        $('.container .unit .hide_arrow').not('.Selected').removeClass('open');
        $('.container .unit .items').not('.Selected').slideUp(300);
        $('.container .unit .title_container').not('.Selected').removeClass('open');
    })
}
//控制-------------------------------------mobile footer
//-----------------------------------------mobile

function ctrl_menu_mobile() {

   $('.control .close_btn').click(function() {

        $('.pc_products_menu').fadeToggle("slow", "linear");
        $('html, body').toggleClass('disable_scroll');
    })


}

//-----------------------------------------mobile
//-----------------------------------------------------pc產品控制
function ctrl_menu_pc() {

    $('.item_container .products').click(function() {
        $('.pc_products_menu').fadeToggle("slow", "linear");
        $('html, body').toggleClass('disable_scroll');
    })
    var subHeight
    var quantity
    var originalHeight = 55;
    $('.item_container .item').mouseover(function() {
        quantity = $(this).find('a.sub').size();
        subHeight = $(this).find('a.sub').eq(0).height();
        $(this).css({ 'height': originalHeight + (quantity * subHeight) + 'px' });
    });
    $('.item_container .item').mouseout(function() {
        $(this).css({ 'height': originalHeight + 'px' });

    })
    $('.control .close_btn').click(function() {

        $('.pc_products_menu').fadeToggle("slow", "linear");
        $('html, body').toggleClass('disable_scroll');
    })
}
$('.product_list_container').not('.open').slideUp();
$('.control .tab').click(function() {
    var nowId = $(this).attr("id");

    $('.control .tab').removeClass("open");
    $(this).addClass("open");
    $('.product_list_container').removeClass('open');
    $('.product_list_container#' + nowId).addClass('open');
    $('.product_list_container').not('.open').slideUp();
    $('.product_list_container.open').slideDown();

})
$('.menuBox .item').click(function() {

    $('.pc_products_menu').fadeToggle("slow", "linear");
    var nowId = $(this).attr("id");
    console.log(nowId);
    $('.control .tab').removeClass("open");
    $('.control #' + nowId).addClass("open");
    $('.product_list_container').removeClass('open');
    $('.product_list_container#' + nowId).addClass('open');
    $('.product_list_container').not('.open').slideUp();
    $('.product_list_container.open').slideDown();
    $('html, body').toggleClass('disable_scroll');

})


//-----------------------------------------------------pc產品控制
//------------------------------------------控制電腦版menu follow
function ctrl_menu_follow() {
    var windowHeight = $(window).height();

    $(window).scroll(function() {

        window_scrollTop = $(window).scrollTop();
        var menuHeight = $('.mainmenu').height();
        if (window_scrollTop >= menuHeight) {
            //console.log(menuHeight);

            $('.mainmenu').addClass("fixedMenu");
            $('.secMenu').addClass("sec");


            if (window_scrollTop >= windowHeight / 2) {

                // $('.mainmenu').addClass("open ");


            } else {

                // $('.mainmenu').removeClass("open");

            }
        } else {

            $('.mainmenu').removeClass("fixedMenu");
            $('.secMenu').removeClass("sec");

        }

    })

}

//------------------------------------------控制電腦版menu follow
function searchCtrl(){

  $('.search').mouseover(function(){
    $(this).find('input').addClass('open');
  });
  $('.search').mouseout(function(){
    $(this).find('input').removeClass('open');
  });
}

//-----------------------------------------------search輸入框效果

//撈出陣列的最大值--------------------------------------------
function array_max() { // 定義求取 Array 最大值之函式
    var i, max = this[0];
    for (i = 1; i < this.length; i++) {
        if (max < this[i])
            max = this[i];
    }
    return max;
}
Array.prototype.max = array_max; // 在 Array 原型中加入 max 函式''

//撈出陣列的最大值----------------------------------------------

function nowSection(nav){
      $('body').prepend('<div class="PdSectionDetect" style="position: fixed;width:1px;height:1px;top:50%;right:50%;z-index:9999;"></div>');
    var detect=$('.PdSectionDetect');
    var navObj=nav;
    var pagesection=$('.pageSection');
    var navQuantity=navObj.find('.items').size();
    var sectionQuantity=$('.pageSection').size();

    
    if(navQuantity==navQuantity){
        console.log("good");
       
        setInterval(function(){
             var collides = pagesection.overlaps(detect);
             var overlapsSection = $(collides.targets).attr('id');
             navObj.find('.items').removeClass("active");
             navObj.find($('.'+overlapsSection+'')).addClass("active");
             //console.log($(collides.targets).index());
        }, 500);
    }else{
        console.log("Navigations button quantity not match of sections quantity")
    }

}

//產品內頁section標記----------------------------------------------
////撈出陣列的最大值--------------------------------------------------------------------------------------------


