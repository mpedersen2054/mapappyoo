
import something from './anotherfile'

$(document).ready(() => {

var map = L.map('map').setView([47.606438, -122.132453], 16);

L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

L.marker([47.606438, -122.132453]).addTo(map)
    .bindPopup('A pretty CSS3 popup.<br> Easily customizable.')
    .openPopup();

// console.log(map.getBounds())

var moves = 0

map.on('move', function(e) {
    moves++
    if (moves % 20 == 0) {
    var nm = map.getBounds()
    var ne = nm._northEast
    var sw = nm._southWest
    console.log(ne.lat, ne.lng)
    console.log(sw.lat, sw.lng)
    console.log(moves)
    }
})

map.on('click', function(ev) {
    console.log(map); // ev is an event object (MouseEvent in this case)
});

})