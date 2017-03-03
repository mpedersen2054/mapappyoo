
$(function() {

function reqLocations(groupId, callback) {
    $.get('/getGroupLocations/' + groupId, function(data) {
        console.log('INSIDE POST REQ!!!')
        console.log(data)
        callback(null, data)
    })
}

reqLocations($('.group-id').data('groupid'), function(err, data) {
    console.log('inside the CB!!!!')
    console.log('err?', err)
    console.log('data:')
    console.log(JSON.parse(data))
})



var map = L.map('map').setView([47.606438, -122.132453], 16);

L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

L.marker([47.606438, -122.132453]).addTo(map)
    .bindPopup('A pretty CSS3 popup.<br> Easily customizable.')
    .openPopup();
})











// var moves = 0
// var ticks = 0

// map.on('move', function(e) {
//     moves++
//     if (moves % 40 == 0) {
//         ticks++
//         var nm = map.getBounds()
//         var ne = nm._northEast
//         var sw = nm._southWest
//         console.log(ne.lat, ne.lng)
//         console.log(sw.lat, sw.lng)
//         console.log(ticks)
//     }
// })

// map.on('click', function(ev) {
//     console.log(map); // ev is an event object (MouseEvent in this case)
// })