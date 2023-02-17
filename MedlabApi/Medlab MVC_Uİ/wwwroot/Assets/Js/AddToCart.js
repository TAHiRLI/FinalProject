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