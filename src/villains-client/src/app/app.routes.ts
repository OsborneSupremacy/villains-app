import { Routes } from '@angular/router';
import { VillainSelectorComponent } from './villain-selector/villain-selector.component';
import {VillainAddComponent} from "./villain-add/villain-add.component";

export const routes: Routes = [
  {
    path: '',
    component: VillainSelectorComponent
  },
  {
    path: 'villain/add',
    component: VillainAddComponent
  }
];
