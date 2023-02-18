$(document).on("click", ".add-to-cart", function (e) {
    e.preventDefault();
    let link = $(this).attr("href");


    fetch(link)
        .then(response => {
            if (!response.ok) {
                //Swal.fire({
                //    title: 'Error!',
                //    text: 'This product is out of stock',
                //    icon: 'error',
                //    confirmButtonText: 'Ok'
                //})
                throw new Error("product out of stock");
                return;
            }
            return response.text();
        })
        .then(data => {
            //Swal.fire({
            //    position: 'center',
            //    icon: 'success',
            //    title: 'Added',
            //    showConfirmButton: false,
            //    timer: 1200
            //})
            $(".BasketMiniPartialHolder").html(data);
            console.log(data)
        })
        .catch(error => {
            console.log(error)
        })


})
//========================
// Remove form cart
//========================

$(document).on("click", ".x-btn", function (e) {
    e.preventDefault();

    let link = $(this).attr("href");

    fetch(link)
        .then(response => {
            if (!response.ok) {
                //Swal.fire({
                //    title: 'Error!',
                //    text: 'This product is out of stock',
                //    icon: 'error',
                //    confirmButtonText: 'Ok'
                //})
                throw new Error("product not found in basket");
                return;
            }
            return response.text();
        })
        .then(data => {
            //Swal.fire({
            //    position: 'center',
            //    icon: 'success',
            //    title: 'Added',
            //    showConfirmButton: false,
            //    timer: 1200
            //})
            $(".BasketMiniPartialHolder").html(data);
        })
        .catch(error => {
            console.log(error)
        })
        .finally(() => {
            if (e.target.classList.contains("reload"))
                window.location.reload();
        })

})


//========================
// Increment or decrement Product Cart Count
//========================

$(document).on("click", ".increment-cart", function (e) {
    e.preventDefault();

    var link = $(this).attr("href")
    console.log(link)


    fetch(link)
        .then(response => {
            if (!response.ok) {
                //Swal.fire({
                //    title: 'Error!',
                //    text: 'This product is out of stock',
                //    icon: 'error',
                //    confirmButtonText: 'Ok'
                //})
                throw new Error("product out of stock");
                return;
            }
            return response.text();
        })
        .then(data => {
            //Swal.fire({
            //    position: 'center',
            //    icon: 'success',
            //    title: 'Added',
            //    showConfirmButton: false,
            //    timer: 1200
            //})
            $(".BasketMiniPartialHolder").html(data);
            console.log(data)
        })
        .catch(error => {
            console.log(error)
        })
        .finally(() => {
            let infolink = "/basket/getbasketinfo"
            fetch(infolink)
                .then(res => res.json())
                .then(data => UpdateBasketInfo(data))
        })



})

$(document).on("click", ".decrement-cart", function (e) {
    e.preventDefault();

    let input = e.target.nextElementSibling;
    let count = parseInt(input.value)
    if (count >= 1) {


        var link = $(this).attr("href")
        console.log(link)


        fetch(link)
            .then(response => {
                if (!response.ok) {
                    return;
                }
                return response.text();
            })
            .then(data => {
                $(".BasketMiniPartialHolder").html(data);
                console.log(data)
            })
            .catch(error => {
                console.log(error)
            })
            .finally(() => {
                let infolink = "/basket/getbasketinfo"
                fetch(infolink)
                    .then(res => res.json())
                    .then(data => UpdateBasketInfo(data))

            })
    }
    else if (count == 0) {
        var element = $(this).parent().parent().children()[0];
        let link = element.getAttribute("href")
        fetch(link)
            .then(response => {
                if (!response.ok) {
                    return;
                }
                return response.text();
            })
            .then(data => {
                $(".BasketMiniPartialHolder").html(data);
                console.log(data)
            })
            .catch(error => {
                console.log(error)
            })
            .finally(() => window.location.reload())
    }



})





//========================
// Add to cart With Count / product-details page
//========================

$(document).on("click", ".addToCartWithCount", function (e) {
    e.preventDefault();

    let link = $(this).attr("href")
    console.log(link)
    let count = $(".add-count").val();

    link += `?count=${count}`

    if (count > 0) {
        fetch(link)
            .then(response => {
                if (!response.ok) {
                    return;
                }
                return response.text();
            })
            .then(data => {
                $(".BasketMiniPartialHolder").html(data);
                console.log(data)
            })
            .catch(error => {
                console.log(error)
            })

    }

})



const UpdateBasketInfo = (data)=>{
    $(".sub-total").html(`$${(data.subtotal).toFixed(2)}`)
    $(".total").html(`$${(data.total).toFixed(2)}`)
    $(".discounted").html(`$${(data.subtotal - data.total).toFixed(2)}`)
}