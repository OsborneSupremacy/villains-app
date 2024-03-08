import { Component } from '@angular/core';
import {RouterLink, RouterLinkActive} from "@angular/router";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [
    RouterLink,
    NgClass,
    RouterLinkActive
  ],
  templateUrl: './nav-menu.component.html',
  styleUrl: './nav-menu.component.scss'
})
export class NavMenuComponent {

  public isExpanded: boolean;

  constructor() {
    this.isExpanded = false;
  }

  ngOnInit(): void {
  }

  public collapse() {
    this.isExpanded = false;
  }

  public toggle() {
    this.isExpanded = !this.isExpanded;
  }

}
