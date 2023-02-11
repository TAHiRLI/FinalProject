$('.add-photo').on('click', function(e) {
    e.preventDefault();
    console.log('salidil')
    $('#user-photo-input').trigger('click');
  });

  //  enable user to change uservalues when edit button is clicked

  $(".edit-btn").on("click", function(e){
    $('.user-input').removeAttr("disabled");
   $(this).addClass("d-none");
   $(".save-btn").removeClass("d-none")
  })

  