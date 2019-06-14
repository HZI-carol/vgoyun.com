var tabId = 1

$(window).ready(function () {
  watch()
  $(window).scroll(function() {
    watch()
  })
  

  var mySwiper = new Swiper ('.module-slider .swiper-container', {
    direction: 'horizontal', // 垂直切换选项
    loop: true, // 循环模式选项
    // 如果需要分页器
    pagination: {
      el: '.swiper-pagination',
    },
    
    // 如果需要前进后退按钮
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
  })  
  var moreRealSwiper = new Swiper('#more-real .swiper-container', {
    watchSlidesProgress: true,
    slidesPerView: 'auto',
    centeredSlides: true,
    loop: true,
    loopedSlides: 5,
    autoplay: true,
    // navigation: {
    //   nextEl: '.swiper-button-next',
    //   prevEl: '.swiper-button-prev',
    // },
    // pagination: {
    //   el: '.swiper-pagination',
    //   //clickable :true,
    // },
    on: {
      progress: function(progress) {
        for (i = 0; i < this.slides.length; i++) {
          var slide = this.slides.eq(i);
          var slideProgress = this.slides[i].progress;
          
          modify = 1;
          // if (Math.abs(slideProgress) > 1) {
          //   modify = (Math.abs(slideProgress) - 2) * 0.1 + 1;
          // }
          translate = slideProgress * modify * 300 + 'px';
          // translate = 50 + 'vw';
          scale = 1 - Math.abs(slideProgress) / 5;
          zIndex = 999 - Math.abs(Math.round(10 * slideProgress));
          slide.transform('translateX(' + translate + ') scale(' + scale + ')');
          slide.css('zIndex', zIndex);
          slide.css('opacity', 1);
          if (Math.abs(slideProgress) > 3) {
            slide.css('opacity', 0);
          }
        }
      },
      setTransition: function(transition) {
        for (var i = 0; i < this.slides.length; i++) {
          var slide = this.slides.eq(i)
          slide.transition(transition);
        }
  
      }
    }
  
  })
  var moreSmartSwiper = new Swiper('#more-smart .swiper-container', {
    watchSlidesProgress: true,
    slidesPerView: 'auto',
    centeredSlides: true,
    loop: true,
    loopedSlides: 5,
    autoplay: true,
    // navigation: {
    //   nextEl: '.swiper-button-next',
    //   prevEl: '.swiper-button-prev',
    // },
    // pagination: {
    //   el: '.swiper-pagination',
    //   //clickable :true,
    // },
    on: {
      progress: function(progress) {
        for (i = 0; i < this.slides.length; i++) {
          var slide = this.slides.eq(i);
          var slideProgress = this.slides[i].progress;
          modify = 1;
          if (Math.abs(slideProgress) > 1) {
            modify = (Math.abs(slideProgress) - 2) * 0.3 + 1;
          }
          translate = slideProgress * modify * 600 + 'px';
          scale = 1 - Math.abs(slideProgress) / 5;
          zIndex = 999 - Math.abs(Math.round(10 * slideProgress));
          slide.transform('translateX(' + translate + ') scale(' + scale + ')');
          slide.css('zIndex', zIndex);
          slide.css('opacity', 1);
          if (Math.abs(slideProgress) > 3) {
            slide.css('opacity', 0);
          }
        }
      },
      setTransition: function(transition) {
        for (var i = 0; i < this.slides.length; i++) {
          var slide = this.slides.eq(i)
          slide.transition(transition);
        }
  
      }
    }
  
  })
})
function watch() {
  getStatus($(".smart-city>div[class^='section']"))
  getStatus($(".section-summarize .top"))
  getStatus($(".section-summarize .bottom"))
  getStatus($(".section-partner .content ul li.item"))
  getStatus($(".section-function .content>div"))
}

function getStatus(Dom) {
  var windowHeight = $(window).height();   //获取浏览器窗口高度
  for (let i = 0; i < Dom.length ; i++) {
    if ($(document).scrollTop() >= $(Dom[i]).offset().top - windowHeight/1.4) {
      $(Dom[i]).addClass('on')
    }
  }
}

function removeOn(Dom) {
  for (let i = 0; i < Dom.length ; i++) {
    $(Dom[i]).removeClass('on')
  }
}


