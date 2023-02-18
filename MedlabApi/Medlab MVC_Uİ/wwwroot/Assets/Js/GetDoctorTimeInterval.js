$("#dateInput").on("change", function (e) {
    let doctorId = $("#timeIntervalSelecBox").attr("DoctorId");
    console.log($(this).val());

    const selectedDate = new Date($(this).val());
    let year = selectedDate.getFullYear();
    let month = selectedDate.getMonth()+1;
    let day = selectedDate.getDate();

    getAvailableTime(year, month, day)

})


var dtToday = new Date();

var month = dtToday.getMonth() + 1;
var day = dtToday.getDate();
var year = dtToday.getFullYear();
if (month < 10)
    month = '0' + month.toString();
if (day < 10)
    day = '0' + day.toString();

var maxDate = year + '-' + month + '-' + day;

$("#dateInput").attr("min", maxDate)



const getAvailableTime = ( year, month, day) => {
    let doctorId = $("#timeIntervalSelecBox").attr("DoctorId");

    let link = `/doctor/getAvailableTime/${doctorId}?year=${year}&&month=${month}&&day=${day}`;

    fetch(link)
        .then(res => res.json())
        .then(data => {
            $('#timeIntervalSelecBox').html("")
            data.forEach(item => {
                $("#timeIntervalSelecBox").append(new Option(item, item));
            })

        })

}

getAvailableTime(year, month, day)