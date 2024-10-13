import { Component } from '@angular/core';
import { AfterViewInit, Input, OnDestroy, OnInit } from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-map-view',
  standalone: true,
  imports: [],
  template: '<div [id]="mapId" style="height: 200px; width: 100%"></div>',
  styleUrl: './map-view.component.scss'
})
export class MapViewComponent {
  @Input  () coordinates: [number, number] | null = null;

  private map: L.Map;
  private marker: L.Marker;

  // Generate a unique ID for each map instance
  mapId = 'map' + Math.random().toString(36).substr(2, 9);

  ngOnInit() {
    const iconRetinaUrl = 'assets/marker-icon-2x.png';
    const iconUrl = 'assets/marker-icon.png';
    const shadowUrl = 'assets/marker-shadow.png';
    L.Marker.prototype.options.icon = L.icon({
      iconRetinaUrl,
      iconUrl,
      shadowUrl,
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      tooltipAnchor: [16, -28],
      shadowSize: [41, 41],
    });
  }

  ngAfterViewInit() {
    this.initMap();
  }

  private initMap(): void {
    // Set default coordinates if none are provided
    const defaultCoords: [number, number] = [45.815, 15.9819]; // Zagreb coordinates

    // Use provided coordinates or default
    const coords = this.coordinates || defaultCoords;

    this.map = L.map(this.mapId, {
      attributionControl: false,
      zoomControl: false,
      dragging: false,
      scrollWheelZoom: false,
      doubleClickZoom: false,
      boxZoom: false,
      touchZoom: false,
      keyboard: false,
    }).setView(coords, 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors',
    }).addTo(this.map);

    // Add marker if coordinates are provided
    if (this.coordinates) {
      this.setMarker(L.latLng(this.coordinates[0], this.coordinates[1]));
    }
  }

  private setMarker(latlng: L.LatLng): void {
    if (this.marker) {
      this.map.removeLayer(this.marker);
    }
    this.marker = L.marker(latlng).addTo(this.map);
  }

  ngOnDestroy(): void {
    if (this.map) {
      this.map.remove();
    }
  }
}
