$(".star-rating").starRating({
    starSize: 18,
    useFullStars: true,
    hoverColor: "#3368c6",
    ratedColor: "#3368c6",

    callback: function (currentRating, $el) {
      // make a server call here
      console.log(currentRating);
      $('#reviewRate').val(currentRating);
    },
  });