//  Content
// 1 Navbar Search
// 2 Navbar Scroll
// 3 Mobile Menu Toggle
// 4 Cartmini opener






// =============
// 1 Navbar Search
// =============

let searchButton = document.querySelector(".fa-search");
let resultBox = document.querySelector(".search-result");
        let input = document.querySelector(".search-input")

    searchButton.addEventListener("click", (e) => {
        let searchBox = e.target.parentElement;
        if (!input.value?.length > 0)
            searchBox.classList.toggle("active")
        else {
            var values = input.value.split(" ").join("%20")
            let link = "/shop/index?search=" + values;
            location.replace(link)
        }
    })


let search_link = "/product/GetSearchRecommendation?search=";
let timeout = null;

$(document).on("keyup", ".search-input", function (e) {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
        var values = e.target.value.split(" ").join("%20")
        let newLink = search_link + values;

        console.log(newLink);

        fetch(newLink)
            .then(res => res.text())
            .then(data => {
                $("#search-holder").html(data);
            })

    }, 500)
})






// =============
// 2 Navbar Scroll
// =============
window.addEventListener("scroll", () => {
    if (document.body.scrollTop > 400 || document.documentElement.scrollTop > 400) {
        document.getElementById("navbarScroll").style.top = "0";

    }
    else {
        document.getElementById("navbarScroll").style.top = "-20%";
        $(".mobileScroll").removeClass("active");
        $(".cartMiniScroll").removeClass("active");

    }
})

// ===================
// 3 Mobile Menu Toggle
// ===================

$(document).on("click", ".mobileBars", (e) => {
    if (e.target.classList.contains("scroll")) {
        $(".mobileScroll").toggleClass("active");
    }
    else {
        $(".mobileTop").toggleClass("active");
    }
})


//==================
// 4 Cartmini opener
//==================

$(document).on("click", ".fa-bag-shopping", (e) => {
    console.log(console.log(e.target))

    if (e.target.classList.contains("scroll"))
        $(".cartMiniScroll").toggleClass("active");
    else {
        $(".cartMiniTop").toggleClass("active");

    }
})