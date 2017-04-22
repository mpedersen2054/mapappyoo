
$(function() {

var map = L.map('map').setView([47.606438, -122.132453], 16);

L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

function reqLocations(groupId, callback) {
    $.get('/getGroupLocations/' + groupId, function(data) {
        callback(null, data)
    })
}

reqLocations($('.group-id').data('groupid'), function(err, data) {
    var pData = JSON.parse(data)
    var groupLocs = pData.GroupLocs

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
    }
})

})