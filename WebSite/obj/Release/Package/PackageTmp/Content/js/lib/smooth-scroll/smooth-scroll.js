$(function() {
    $('a[href*="#"]:not([href="#"])').click(function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                    $('html, body').animate({
                        scrollTop: target.offset().top - 100
                    }, 0);

                } else {
                    $('html, body').animate({
                        scrollTop: target.offset().top - 100
                    }, 500);



                }


            }
        }
    });
});