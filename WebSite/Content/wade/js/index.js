$(window).load(function() {

        // ----------------------------------------Call the rippler
       
        // ----------------------------------------Call the rippler
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
      
        } else {
            // ----------------------------------------首頁youtube API 控制  
            var tag = document.createElement('script');
            tag.src = "https://www.youtube.com/iframe_api";
            var firstScriptTag = document.getElementsByTagName('script')[0];
            firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

            var player;
            var done = false;
            $('.media').css({ 'background-image': 'none' });
            // ----------------------------------------首頁youtube API 控制
             // ----------------------------------------產品列表 控制  
           
            
             // ----------------------------------------產品列表 API 控制  
        }
    })
    // ----------------------------------------首頁youtube API 控制
function onYouTubeIframeAPIReady() {
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




