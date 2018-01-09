 var isMobile = (function(a) { return /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4)) })(navigator.userAgent || navigator.vendor || window.opera);

 $(document).foundation();
 $(window).load(function() {
             //這裡都是控制產品內容頁的function

             if ($('.products_banneer').length > 0) {
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
             }
         });
         //產品內容相簿----------------------------------------------------------------------

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
         //產品內容相簿----------------------------------------------------------------------
         //計算配件----------------------------------------------------------------------
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
         //計算配件----------------------------------------------------------------------
         //產品頁章節按鈕----------------------------------------------------------------
         function sectionBtn() {
             $('.mainContent').slideUp();
             $('.mobile_section_btn.active').next().slideDown();
             $('.mobile_section_btn').click(function() {
                 $(this).next().slideToggle();
                 //$('.mobile_section_btn').toggleClass('active');
                 $(this).toggleClass('active');

             })

         }
         //產品頁章節按鈕----------------------------------------------------------------
         // -------------------------------------產品頁控制規格的切換
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
         // -------------------------------------產品頁控制規格的切換
         // -------------------------------------產品頁控制規格的切換
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
         // -------------------------------------產品頁控制規格的切換
         // -------------------------------------產品頁點擊大圖
         function PicZoom() {
             $('#containerLargePic').zoom();
         }
         // -------------------------------------產品頁點擊大圖
         // -------------------------------------產品頁配件slider
         function AccessoriesSlider() {


             $(document).ready(function() {
                 var owl = $('.owl-carousel');
                 owl.owlCarousel({
                     nav: false,
                     items: 1,
                     margin: 30,
                     URLhashListener: true,
                     autoplayHoverPause: true,
                     smartSpeed: 0,
                     startPosition: 'URLHash'
                     //rtl:true,
                 });
                 owl.on('changed.owl.carousel', function(event) {

                     var page = event.page.index;
                     if (page == 0) {
                         $('#AccessoriesOpen .prevBtn').addClass('disable');
                         $('#AccessoriesOpen .nextBtn').removeClass('disable');
                     } else if (page == $('.owl-item').size() - 1) {
                         $('#AccessoriesOpen .nextBtn').addClass('disable');
                         $('#AccessoriesOpen .prevBtn').removeClass('disable');
                     } else {

                         $('#AccessoriesOpen .nextBtn').removeClass('disable');
                         $('#AccessoriesOpen .prevBtn').removeClass('disable');


                     }
                     // console.log(page);

                 })
                 $('#AccessoriesOpen .nextBtn').click(function() {
                     owl.trigger('next.owl.carousel', [300]);
                 })
                 $('#AccessoriesOpen .prevBtn').click(function() {
                     owl.trigger('prev.owl.carousel', [300]);
                 })

             });
         }
         // -------------------------------------產品頁配件slider
         //-------------------產品頁------------判斷規格的tab數量

         $(window).load(function() {

             console.log($('.spc_tab a').size());
             if ($('.spc_tab a').size() == 1) {
                 $('.spc_tab a').removeClass("small-6");
                 $('.spc_tab a').addClass("small-12");
             }
         })
         //--------------------產品頁-----------判斷規格的tab數量

         //--------------------產品頁-----------判斷配件章節數量
         $(window).load(function() {
             var sectionQuantity = $('section').size();
             console.log("sectionQuantity=" + sectionQuantity);
             $('section').eq(sectionQuantity - 2).addClass('lastSection');
         })

         //--------------------產品頁-----------判斷配件章節數量

         var loadComplete = function() {
             $('#loader').css({ "display": "none", "left": 100 + "%" });

         }

         //--------------------產品頁---------------------------------控制輸入框
         $(window).load(function() {
             // Run code
             //detect mobile or pc
             //---------------------------------------------------------------imagesLoaded
             if ($('#SR').length >= 1) {
                 var tween_loader = TweenMax.to($('#loader'), 0.6, {
                     opacity: .0,
                     onComplete: loadComplete
                 })

             } else {
                 $('body').imagesLoaded()
                     .always(function(instance) {
                         console.log('all images loaded');

                     })
                     .done(function(instance) {
                         console.log('all images successfully loaded');
                         var tween_loader = TweenMax.to($('#loader'), 0.6, {
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
             }
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

             // }
             if ($(window).width() <= 1000) {
                 crt_clickMenu();
                 //ctrl_footer();
                 ctrl_menu_mobile();
             } else {
                 ctrl_menu_pc();
                 ctrl_menu_follow();
                 searchCtrl();

             }
             ctrlSelectBox();
             // ------------------------------------------------------top鍵控制
             if ($(this).scrollTop() < 200) {
                 $('.gotop').fadeOut();
             }
             $(window).on('scroll', function() {
                 if ($(this).scrollTop() > 200) {
                     $('.gotop').fadeIn();
                 } else {
                     $('.gotop').fadeOut();
                 }
             });

             $('.gotop').click(function() {
                 $("html, body").animate({ scrollTop: 0 }, 300);
                 return false;
             });

             var controller = new ScrollMagic.Controller();
             if (isMobile) {

             } else {

                 //alert("noMobile!");

                 if ($('.page-service-link').length >= 1 && $('.noUnderMenu').length == 0) {
                     console.log('uaeuaeuae');

                     new ScrollMagic.Scene({
                             triggerElement: ".page-service-link",
                             offset: -420
                         })
                         .setClassToggle(".gotop", "pin")
                         //.addIndicators()
                         .addTo(controller); // assign the scene to the controller
                 } else {
                     if ($('.pdContent.service').length >= 1) {
                         console.log('xiexiexiexie');
                         new ScrollMagic.Scene({
                                 triggerElement: ".pdContent",
                                 offset: -400
                             })
                             .setClassToggle(".gotop", "pin")
                             //.addIndicators()
                             .addTo(controller); // assign the scene to the controller
                     } else {
                         if ($(window).width() <= 1024) {

                             new ScrollMagic.Scene({
                                     triggerElement: "#FtContainer",
                                     offset: -560
                                 })
                                 .setClassToggle(".gotop", "pin")
                                 .addTo(controller);


                         } else if ($(window).width() <= 780) {

                             new ScrollMagic.Scene({
                                     triggerElement: "#FtContainer",
                                     offset: -90
                                 })
                                 .setClassToggle(".gotop", "pin")
                                 .addTo(controller);


                         } else if ($(window).width() >= 1025) {

                             new ScrollMagic.Scene({
                                     triggerElement: "#FtContainer",
                                     offset: -500
                                 })
                                 .setClassToggle(".gotop", "pin")
                                 .addTo(controller);
                         }
                     }
                 }
             }

             // ------------------------------------------------------top鍵控制
             // -----------------------------------------------------首頁影片
             if (isMobile) {
                 // console.log("now mobile");
                 console.log("now mobile");
             } else {
                 // ----------------------------------------首頁youtube API 控制  
                 if ($('#indexMedia').length > 0) {
                     console.log("now pc");
                     var tag = document.createElement('script');
                     tag.src = "https://www.youtube.com/iframe_api";
                     var firstScriptTag = document.getElementsByTagName('script')[0];
                     firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

                     var player;
                     var done = false;
                     $('.media').css({ 'background-image': 'none' });
                 }

                 // ----------------------------------------首頁youtube API 控制
             }
             if ($('.index_banner').length > 0) {
                 initSlider();
             }


             // -----------------------------------------------------首頁影片 


         });








         //-----------------------------------------------------控制首頁影片
         function initSlider() {


             var SliderOption = {
                 speed: 800,
                 viewport: 'fill',
                 autoplay: 'true',
                 preloadMode: 'all',
                 navigation: {
                     keyboardNavBy: 'slides',
                     horizontal: 1, // Switch to horizontal mode.
                     navigationType: 'basic', // Slide navigation type. Can be: 'basic', 'centered', 'forceCentered'.
                     slideSelector: null, // Select only slides that match this selector.
                     smart: 1, // Repositions the activated slide to help with further navigation.
                     activateOn: 'click', // Activate an slide on this event. Can be: 'click', 'mouseenter', ...
                     activateMiddle: 1, // Always activate the slide in the middle of the FRAME. forceCentered only.
                     slideSize: 0, // Sets the slides size. Can be: Fixed(500) or Percent('100%') number.
                     keyboardNavBy: null,
                     slideSize: '100%'
                 },
                 pages: {
                     // Selector or DOM element for pages bar container.
                     activateOn: 'click'

                 },
                 dragging: {
                     swingSpeed: 0.1
                 },
                 cycling: {
                     cycleBy: 'slides', // Enable automatic cycling by 'slides' or 'pages'.
                     pauseTime: 5000, //Delay between cycles in milliseconds.
                     loop: 1, // Repeat cycling when last slide/page is activated.
                     pauseOnHover: 0, // Pause cycling when mouse hovers over the FRAME.
                     startPaused: 0 // Whether to start in paused sate.
                 },
                 commands: {
                     thumbnails: 0, // Enable thumbnails navigation.
                     pages: 1, // Enable pages navigation.
                     buttons: 1 // Enable navigation buttons.
                 }



             }
             var NewIndexSlider = new mightySlider(
                 ".frame", SliderOption, {
                     //active:activePageAnimation
                 }).init();

             function activePageAnimation(eventName, pageIndex) {
                 console.log(eventName);
                 console.log(pageIndex);

             }
         }

         function onYouTubeIframeAPIReady() {
             if (isMobile) {

             } else {

                 var videoName = $('#video_name').text();
                 console.log(videoName);
                 player = new YT.Player('index_video', {
                     height: '100%',
                     width: '100%',
                     videoId: videoName,
                     playerVars: {
                         'controls': 0,
                         'modestbranding': 0,
                         'rel': 0,
                         'showinfo': 0

                     },
                     events: {
                         'onReady': onPlayerReady,
                         'onStateChange': onPlayerStateChange
                     }
                 });
             }
         }

         function onPlayerReady(event) {
             event.target.playVideo();
             event.target.mute();
             event.target.setLoop(1);
         }

         function onPlayerStateChange(event) {
             if (event.data == YT.PlayerState.ENDED) {
                 player.playVideo();
             }
         }

         function stopVideo() {
             player.stopVideo();
         }
         //-----------------------------------------------------控制首頁影片

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
             $('#ClickMenu').slideUp(0);
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
                 $('#ClickMenu').slideToggle(300);
                 $('.gotop.noUnderMenu').toggleClass('hideTop');
             })

             $('.list_cover').click(function() {
                 console.log('haha');
             })
         }
         //控制-------------------------------------mobile選單
         //控制-------------------------------------mobile footer

         function ctrl_footer() {
             $('#FtContainer .unit').find('.items').slideUp(0);
             $('#FtContainer .unit').click(function() {
                 $('#FtContainer .unit #HideArrow').removeClass('Selected');
                 $('#FtContainer .unit .items').removeClass('Selected');
                 $('#FtContainer .unit .title_container').removeClass('Selected');

                 $(this).find('#HideArrow').toggleClass('open');
                 $(this).find('#HideArrow').addClass('Selected');
                 $(this).find('.items').slideToggle(300);
                 $(this).find('.items').addClass('Selected');
                 $(this).find('.title_container').toggleClass('open');
                 $(this).find('.title_container').addClass('Selected');

                 $('#FtContainer .unit #HideArrow').not('.Selected').removeClass('open');
                 $('#FtContainer .unit .items').not('.Selected').slideUp(300);
                 $('#FtContainer .unit .title_container').not('.Selected').removeClass('open');
             })
         }
         //控制-------------------------------------mobile footer
         //-----------------------------------------mobile

         function ctrl_menu_mobile() {

             $('.control .close_btn').click(function() {

                 $('#pcProductsMenu').fadeToggle("fast");
                 $('html, body').toggleClass('disable_scroll');
                 console.log(OriginalScrollTop);
                 $(window).scrollTop(OriginalScrollTop);
             })


         }
         OriginalScrollTop = null; //建立全域變數儲存scroll的值
         //-----------------------------------------mobile
         //-----------------------------------------------------pc產品控制
         function ctrl_menu_pc() {
             if ($('.index_banner').length >= 1) {

                 oringalCollection = 9999;

             } else {

                 var oringalCollection = $('#item_container .active').index(); //偵測當前的單元
                 console.log(oringalCollection);


             }
             console.log("oringalCollection=" + oringalCollection)


             //控制主選單商品按鈕的行為
             $('#item_container .products').click(function() {

                 //console.log($(window).scrollTop());
                 if (OriginalScrollTop == null || $(window).scrollTop() != 0) {

                     OriginalScrollTop = $(window).scrollTop();
                 }


                 $('#pcProductsMenu').fadeToggle("slow");
                 $('html, body').toggleClass('disable_scroll');
                 $(this).toggleClass('active');
                 $('#item_container .item').eq(oringalCollection).toggleClass('active');

                 $(window).scrollTop(OriginalScrollTop);

             })
             var subHeight
             var quantity
             var originalHeight


             setInterval(function() {
                 if ($('.mainmenu').hasClass('fixedMenu')) {

                     $('#item_container .item').addClass('fixedMenu');

                 } else {
                     $('#item_container .item').removeClass('fixedMenu');

                 }

             }, 500);


             $('#item_container .item').mouseover(function() {
                 if ($('.mainmenu').hasClass('fixedMenu')) {
                     originalHeight = 45;
                 } else {
                     originalHeight = 55;
                 }
                 quantity = $(this).find('a.sub').size();
                 subHeight = $(this).find('a.sub').eq(0).height();
                 $(this).css({ 'height': originalHeight + (quantity * subHeight) + 'px' });
             });
             $('#item_container .item').mouseout(function() {
                 //$(this).css({ 'height': originalHeight + 'px' });
                 $(this).attr({ style: "" });

             })
             $('.control .close_btn').click(function() {

                 $('#pcProductsMenu').fadeToggle("fast");
                 $('html, body').toggleClass('disable_scroll');
                 $('#item_container .item').eq(oringalCollection).toggleClass('active');
                 $('#item_container .products').toggleClass('active');
                 //console.log(OriginalScrollTop);
                 $(window).scrollTop(OriginalScrollTop);
             })
         }
         $('.product_list_container').not('.open').slideUp(); $('.control .tab').click(function() {
             var nowId = $(this).attr("id");

             $('.control .tab').removeClass("open");
             $(this).addClass("open");
             $('.product_list_container').removeClass('open');
             $('.product_list_container#' + nowId).addClass('open');
             $('.product_list_container').not('.open').slideUp();
             $('.product_list_container.open').slideDown();

         }) 
         $('#MenuBox .item,#FooterItems a').click(function() {
             $('#item_container .products').toggleClass('active');
             if (OriginalScrollTop == null || $(window).scrollTop() != 0) {

                 OriginalScrollTop = $(window).scrollTop();
                 if ($('.mainmenu').hasClass('fixedMenu')) {
                     originalHeight = 45;
                 } else {
                     originalHeight = 55;
                 }
             }

             $('#pcProductsMenu').fadeToggle("fast");
             var nowId = $(this).attr("id");
             //console.log(nowId);
             $('.control .tab').removeClass("open");
             $('.control #' + nowId).addClass("open");
             // $('.product_list_container').removeClass('open');
             // $('.product_list_container#' + nowId).addClass('open');
             $('.product_list_container').not('#' + nowId).slideUp(0);
             $('.product_list_container#' + nowId).slideDown(0);
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
         function searchCtrl() {

             $('.search').mouseover(function() {
                 $(this).find('input').addClass('open');
             });
             $('.search').mouseout(function() {
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
         //產品業內判斷當前的章節----------------------------------------

         function nowSection(nav) {
             if ($(window).width() >= 1025) {
                 $('body').prepend('<div id="PdSectionDetect" style="position: fixed;width:1px;height:1px;top:50%;right:50%;z-index:9999;"></div>');
             } else if ($(window).width() <= 1024) {
                 $('body').prepend('<div id="PdSectionDetect" style="position: fixed;width:1px;height:1px;top:20vh;right:50%;z-index:9999;"></div>');

             }

             var detect = $('#PdSectionDetect');
             var navObj = nav;
             var pagesection = $('.pageSection');
             var navQuantity = navObj.find('.items').size();
             var sectionQuantity = $('.pageSection').size();


             if (navQuantity == navQuantity) {
                 //console.log("good");

                 setInterval(function() {
                     var collides = pagesection.overlaps(detect);
                     var overlapsSection = $(collides.targets).attr('id');
                     if (!overlapsSection) {

                     } else {

                         console.log(overlapsSection);
                         navObj.find('.items').removeClass("active");
                         navObj.find($('#' + overlapsSection.toUpperCase() + '')).addClass("active");
                         console.log($('#' + overlapsSection.toUpperCase() + ''));



                     }

                 }, 600);
             } else {
                 console.log("Navigations button quantity not match of sections quantity")
             }

         }
         //產品業內判斷當前的章節----------------------------------------
         //聯絡我們表單的focus問題優化-----------------------------------
         $(function() {
             if ($('.contact-content').length >= 1) {
                 $('form .row').not('.address, .takeCatalogs').click(
                     function() {
                         $(this).find('input').focus();
                         $(this).find('textarea').focus();

                     })

             }

         })
         //聯絡我們表單的focus問題優化-----------------------------------

         //處理select-text選單-----------------------------------

         function ctrlSelectBox() {
             if ($('.select-text').length > 0) {

                 $('.select-text').on('click', function(event) {
                     //event.preventDefault();
                     $(this).siblings('.select-option').slideDown(200);
                 });
                 $('.select-option').on('click', 'li', function(event) {
                     event.preventDefault();
                     var _text = $(this).html();
                     $(this).parents('.select-option').siblings('.select-text').html(_text)
                     $(this).parents('.select-option').fadeToggle(200);
                     $(this).toggleClass('active').siblings().removeClass('active');
                 });

                 $('.select').on('click', function(event) {
                     //event.preventDefault();
                     $(this).addClass('active').siblings().removeClass('active');
                     $(this).siblings().find('.select-option').fadeOut(200);
                 });


                 if (isMobile) {
                     $(document).on('touchstart', function(e) {
                         var container = $(".select-option");
                         if (!$(event.target).closest(container).length) {
                             container.fadeOut(200);
                         }

                     })
                 } else {
                     $(document).mouseup(function(e) {
                         var container = $(".select-option");

                         // if the target of the click isn't the container nor a descendant of the container
                         if (!$(event.target).closest(container).length) {
                             container.fadeOut(200);
                         }
                     });

                 }






             }






         }






















         //處理select-text選單-----------------------------------