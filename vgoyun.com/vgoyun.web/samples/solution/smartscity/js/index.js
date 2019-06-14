//header
$(window).ready(function () {
  $(window).scroll(function() {
    if ($(window).scrollTop() > 100) {
        $(".module-header").css({ 'backgroundColor': 'rgba(0, 0, 0, 0.9)' });
    }else {
      $(".module-header").css({ 'backgroundColor': 'rgba(0, 0, 0, 0.4)' });
    }
  })
  // $(window).scroll(function () {
  //     if ($(window).scrollTop()>80) {
  //         $("header").addClass("header-bg");
  //     }
  //     else{
  //         $("header").removeClass("header-bg");
  //     }
  // })
  //menu-icon
  // $('header .menu-icon').click(function () {
  //     if ($('header .head-menu>ul').css('display') == "none") {
  //         $(".head-menu>ul").css({ 'display': 'block' }).animate({ marginLeft: '50vw', width: '50vw' });
  //         $(".head-menu").animate({display:'block'});
  //         $('header .menu-icon > span').each(function (index) {
  //             $('header .menu-icon > span').eq(index).addClass('span' + (index + 1) + '-animation');
  //         });

  //     } else {
  //         $(".head-menu>ul").animate({ marginLeft: '100vw', width: '0vw' },function () {
  //             $(".head-menu>ul").css({ 'display': 'none' });
  //         });
  //         $('header .menu-icon > span').each(function (index) {
  //             $('header .menu-icon > span').eq(index).removeClass('span' + (index + 1) + '-animation');
  //         });
  //     }
  // });
})