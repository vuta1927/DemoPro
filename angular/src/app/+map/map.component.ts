import { Component, Directive, Input, OnInit } from '@angular/core';
import { forEach } from '@angular/router/src/utils/collection';
declare var google: any;
@Component({
    selector: 'app-map',
    templateUrl: './map.component.html',
    styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {

    originLocation = { lat: 21.017080, lng: 105.781150 };
    destination = { lat: 21.016267, lng: 105.780063 };
    markerList = [];
    drawingManager = new google.maps.drawing.DrawingManager();
    constructor() {
    }

    public ngOnInit() {
        this.initMap();
    }

    initMap() {

        let map = new google.maps.Map(document.getElementById('gmap'), {
            zoom: 18,
            center: this.originLocation,
        });

        var geocoder = new google.maps.Geocoder;
        var infowindow = new google.maps.InfoWindow;
        this.drawLine(map, '#FF0000', [this.originLocation, this.destination]);
    }
    drawLine(map, color, flightPlanCoordinates) {
        var flightPath = new google.maps.Polyline({
            path: flightPlanCoordinates,
            geodesic: true,
            strokeColor: color,
            strokeOpacity: 0.5,
            strokeWeight: 5
        });
        this.markerList.push(this.createMaker(map, flightPlanCoordinates));
        flightPath.setMap(map);
        return flightPath;
    }
    createMaker(map, listLatLng) {
        var markerList = []
        listLatLng.forEach(element => {
            var marker = new google.maps.Marker({
                position: element,
                map: map
            });
            markerList.push(marker);
        });
        return markerList;
    }
    getGeoNameFromLatLng(geocoder, map, infowindow, latLng) {
        geocoder.geocode({ 'location': latLng }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    // map.setZoom(11);
                    var marker = new google.maps.Marker({
                        position: latLng,
                        map: map
                    });
                    infowindow.setContent(results[0].formatted_address);
                    infowindow.open(map, marker);
                } else {
                    window.alert('No results found');
                }
            } else {
                window.alert('Geocoder failed due to: ' + status);
            }
        });
    }
}