
//  1 Quantity field for cart products
// =====================================
$(".increment").on("click", (e)=>{
    let input = e.target.previousElementSibling
    input.value  = parseInt(input.value) + 1;
})

$(".decrement").on("click", (e)=>{
    let input = e.target.nextElementSibling;
    let count =parseInt(input.value)
    if(count >=1)
    input.value  = parseInt(input.value) - 1;
})

