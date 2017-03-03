
$(function() {

var map = L.map('map').setView([47.606438, -122.132453], 16);

L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

L.marker([47.606438, -122.132453]).addTo(map)
    .bindPopup('A pretty CSS3 popup.<br> Easily customizable.')
    .openPopup();
// })


function reqLocations(groupId, callback) {
    $.get('/getGroupLocations/' + groupId, function(data) {
        // console.log('INSIDE POST REQ!!!')
        // console.log(data)
        callback(null, data)
    })
}

reqLocations($('.group-id').data('groupid'), function(err, data) {
    var pData = JSON.parse(data)
    var groupLocs = pData.GroupLocs

    console.log(groupLocs)

    for (var i = 0; i < groupLocs.length; i++) {
        var curr = groupLocs[i].GroupLoc

        var tmpl = `
            <div className="popup">
                <div class="popup-title">${curr.Name}</div>
                <div class="popup-loc">${curr.StreetAdr}, ${curr.City} ${curr.Zip}</div>
                <a href="/locations/${curr.LocationId}">View Page/Ratings</a>
            </div>
        `

        L.marker([curr.Lat, curr.Lng]).addTo(map)
            .bindPopup(tmpl)
            .openPopup();
        // })
    }
    // console.log(pData)
})

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