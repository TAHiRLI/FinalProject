
//  Content                           / Index
// 1 Main Slider                      / Index
// 2 services slider                  / Index
// 3  Doctors slider                  / Index
// 4 Core Values Slider               / About Us 
// 5 Office slider                    / About Us 
// 7 Product Images Slider            /Details
// 8 Similar Products Slider          / Product-Details






//=================================
// 1 Main Slider page/index
//=================================

$(document).ready(function () {
    $('.mainSlider').slick({
        autoplay: true,
        autoplaySpeed: 3000,
        dots: true,
        dotClass: 'slick-dots',
        infinite: true,
        speed: 600,
        pauseOnHover:false,
        fade: true,
        cssEase: 'linear',
        arrows: false,
    });
});


//=================================
// 2 services slider
//=================================

$(document).ready(function () {
    $('.tests_sliderNav').slick({
      autoplay: true,
      autoplaySpeed: 3000,
        infinite:true,
        draggabe:true,
        touchMove:true,
        dotClass: 'slick-dots',
        slidesToShow: 6,
        slidesToScroll: 1,
        asNavFor: '.tests_sliderFor',
        dots: false,
        centerMode: false,
        focusOnSelect: true,
        nextArrow: $(".next_button"),
        prevArrow: $(".prev_button"),
        responsive: [
          {
            breakpoint: 1200,
            settings: {
              slidesToShow: 5,
            }
          },
          {
            breakpoint: 992,
            settings: {
              slidesToShow: 4,
            }
          },
          {
            breakpoint: 768,
            settings: {
              slidesToShow: 3,
            }
          }
          ,
          {
            breakpoint: 560,
            settings: {
              slidesToShow: 2,
            }
          }
        
        ]

    });
});

$('.tests_sliderFor').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    centerMode: false,
    draggabe:false,
    touchMove:false,
    fade: true,
    speed: 0,
    asNavFor: '.tests_sliderNav'
  });

//=================================
// 3  Doctors slider
//=================================

  $('.doctorSlider').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    autoplay: true,
    centerMode:false,
    autoplaySpeed: 2000,
    arrows:false,
    responsive: [
     
      {
        breakpoint: 992,
        settings: {
          slidesToShow: 2,
        }
      },
      {
        breakpoint: 768,
        settings: {
          slidesToShow: 1,
        }
      }
  
    
    ]
  });

//=================================
// 4 Core Values Slider / About Us Page
//=================================
$('.valuesSlider').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    autoplay: true,
    centerMode:false,
    autoplaySpeed: 2000,
    arrows: false,
    responsive: [

        {
            breakpoint: 992,
            settings: {
                slidesToShow: 2,
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 1,
            }
        }


    ]
  });

//=================================
// 5 Office slider
//=================================

$(document).ready(function () {
    $('.galerySliderFor').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        draggable:true,
        fade: true,
        speed: 0,
        arrows:true,
        asNavFor: '.galerySliderNav',
        prevArrow:"<button type='button' class='slick-prev slick-button pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow:"<button type='button' class='slick-next slick-button pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
     
      });


 
});


   $('.galerySliderNav').slick({
        infinite:true,
        draggabe:true,
        touchMove:true,
        dotClass: 'slick-dots',
        slidesToShow: 4,
        slidesToScroll: 1,
         asNavFor: '.galerySliderFor',
        dots: false,
        centerMode: false,
        focusOnSelect: true,
        arrows:true,
        prevArrow:"<button type='button' class='slick-prev slick-button pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow:"<button type='button' class='slick-next slick-button pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
        responsive: [
     
          {
            breakpoint: 992,
            settings: {
              slidesToShow: 3,
            }
          },
          {
            breakpoint: 768,
            settings: {
              slidesToShow: 2,
            }
          }
      
        
        ]
    });





//=================================
// 7 Product Images Slider /Details
//=================================

$(document).ready(function () {
  $('.productNavImgSlider').slick({
      infinite:true,
      draggabe:true,
      touchMove:true,
      dotClass: 'slick-dots',
      slidesToShow: 5,
      slidesToScroll: 1,
      asNavFor: '.productMainImgSlider',
      dots: false,
      centerMode: false,
      focusOnSelect: true,
      autoplay: false,
      arrows:false,
      autoplaySpeed: 4000,
      
  responsive: [
    
    
    {
      breakpoint: 768,
      settings: {
        slidesToShow: 4,
      }
    }
  
  ]


  });
});

$('.productMainImgSlider').slick({
  slidesToShow: 1,
  slidesToScroll: 1,
  arrows: false,
  fade: true,
  speed: 200,
  asNavFor: '.productNavImgSlider'
});


//=================================
// 8 Similar Products Slider / Product-Details
//=================================



$('.similarPrdSlider').slick({
  slidesToShow: 4,
  slidesToScroll: 1,
  autoplay: true,
  centerMode:false,
  autoplaySpeed: 4000,
  arrows:false,
  draggabe:true,
  focusOnSelect: false,
  responsive: [
    {
      breakpoint: 1024,
      settings: {
        slidesToShow: 3,
      }
    },
    {
      breakpoint: 768,
      settings: {
        slidesToShow: 2,
      }
    }
    ,
    {
      breakpoint: 560,
      settings: {
        slidesToShow: 1,
      }
    }
  
  ]
});


