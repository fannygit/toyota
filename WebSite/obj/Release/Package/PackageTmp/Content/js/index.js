 var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
 $(window).load(function() {


     // ----------------------------------------Call the rippler

     // ----------------------------------------Call the rippler
     if (isMobile) {
         // console.log("now mobile");
         console.log("now mobile");
     } else {
         // ----------------------------------------首頁youtube API 控制  
         console.log("now pc");
         var tag = document.createElement('script');
         tag.src = "https://www.youtube.com/iframe_api";
         var firstScriptTag = document.getElementsByTagName('script')[0];
         firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

         var player;
         var done = false;
         $('.media').css({ 'background-image': 'none' });
         // ----------------------------------------首頁youtube API 控制


     }

     initSlider();


 })
 // ----------------------------------------首頁youtube API 控制
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
 // ----------------------------------------首頁youtube API 控制