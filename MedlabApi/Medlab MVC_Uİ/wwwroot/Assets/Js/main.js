//  add to cort quantity

$(".increment").on("click", ()=>{
    $(".add-count").val(parseInt($(".add-count").val())+1)
})


$(".decrement").on("click", ()=>{
    let count =parseInt($(".add-count").val())
    if(count >=1)
        $(".add-count").val(count -1)
})


