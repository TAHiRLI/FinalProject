 
//  Shop Filter slider range for price
 
 $( ".price-slider" ).slider({
           range: true,
           min: 0,
           max: 100,
           values: [ 50,80 ],
           slide: function( event, ui ) {
               $( ".amount" ).html( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
           },
           change: function (event, ui) {
            $(".minPrice").val(ui.values[0]);
            $(".maxPrice").val(ui.values[1]);
        }
        });
        $( ".amount" ).val( "$" + $( ".slider-range" ).slider( "values", 0 ) +
        " - $" + $( ".slider-range" ).slider( "values", 1 ) ); 


