 
let isRTL=false;
$(document).ready( _ => {
    // mobile menu toggle
    let hamburgerMenu=$("#hamburger-menu");
    hamburgerMenu.click( _ =>{
         
        hamburgerMenu.toggleClass('active');
        $("#nav-menu").toggleClass('active');
    });
    // end mobile menu toggle 
    // owl carousel 
    let navText=[
        "<i class='bx bxs-chevron-left ' ></i>",
        "<i class='bx bxs-chevron-right ' ></i>"
    ];
    $("#slide-carousel").owlCarousel({
        
        items:1,
        dots:false,
        loop:true,
        nav:false,
        navText:navText,
        autoplayHoverPause:true,
        autoplay:true,
        rtl:isRTL,
        responsive:{
            720:{
                nav:true,
            }
        }
        
     });
   $('#top-movies').owlCarousel({
   
    items:2,
    dots:false,
    loop:true,
    autoplay:true,
    autoplayHoverPause:true,
    rtl:isRTL,
    responsive:{
        720:{
            items:3,
        },
        1280:{
            items:4,
        },
        1600:{
            items:5,
        },
        1920:{
            items:6,
        }
    }
    });
   $(".movies-slid").owlCarousel({
   
    items:2,
    dots:false,
    loop:true,
    nav:false,
    navText:navText,
    margin:15,
    rtl:isRTL,
    responsive:{
        720:{
            items:3,
            nav:true,
        },
        1280:{
            items:4,
            nav:true,
        },
        1600:{
            items:5,
            nav:true,
        },
        1920:{
            items:6,
            nav:true,
        },
    }
    });
    $('#seasons-slid').owlCarousel({
        items:1,
        loop:false,
        margin:10,
        nav:false,
        rtl:isRTL,
        responsive:{
            361:{
                items:2,
            },
            720:{
                items:3,
            },
            1280:{
                items:5,
            },
            1600:{
                items:8,
            },

        }
    });
    let iframePlayer=$("#iframe-player");
    $(".server-link").click(e=> {
        console.log(e.target.classList);
        $('.server-link.active').removeClass("active");
        e.target.classList.add("active");
        iframePlayer.attr('src',e.target.dataset.link );
    });
     
});

