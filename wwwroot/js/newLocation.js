
// https://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=YOUR_API_KEY

var googGeoApiKey = 'AIzaSyB7U7IV-NrpHuCtBqXAvDp3dFulIb3w50E'

// get/format request string to use for goog geocoding
function getReqStr(streetAdr, city, state, zip) {
    streetAdr = streetAdr.replace(/\s/g, '+')
    city = city.replace(/\s/g, '+')
    state = state.replace(/\s/g, '+')
    return `https://maps.googleapis.com/maps/api/geocode/json?address=${streetAdr},${city},${state}&key=${googGeoApiKey}`
}

// send req to goog geocoord api and return desired data
function requestData(reqStr, callback) {
    var returnData = {}
    $.get(reqStr, function(data) {
        if (data.status == 'OK') {
            var dataObj = data.results[0],
                placeId = dataObj.place_id
                lat = dataObj.geometry.location.lat,
                lng = dataObj.geometry.location.lng

            returnData.lat = lat
            returnData.lng = lng
            returnData.placeId = placeId

            callback(null, returnData)
        } else {
            callback('Data was not valid.', null)
        }
    })
}

var $newLocForm = $('#new-loc-form'),
    $streetAdrInp = $('.streetAdr-inp'),
    $cityInp = $('.city-inp'),
    $stateInp = $('.state-inp'),
    $latInp = $('.lat-coord'),
    $lngInp = $('.lng-coord'),
    $placesIdInp = $('.googlePlacesId')

// when form submitted, get coords from google geo,
// save coords onto hidden elements, submit the form
$newLocForm.on('submit', function(e) {
    e.preventDefault();
    var self = this

    // get the req string for google geocoding
    var reqStr = getReqStr(
        $streetAdrInp.val(),
        $cityInp.val(),
        $stateInp.val()
    )

    requestData(reqStr, function(err, data) {
        if (!err && data) {
            console.log(data)
            $latInp.attr('value', data.lat)
            $lngInp.attr('value', data.lng)
            $placesIdInp.attr('value', data.placeId)

            console.log('inside of requestData cb')

            // avoids endless loop by doing
            // this.submit() instead of $(this).submit()
            self.submit()
        } else {
            // handle not valid data here
            console.log(err)
        }
    })
})