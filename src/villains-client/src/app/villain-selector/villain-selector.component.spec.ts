import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VillainSelectorComponent } from './villain-selector.component';

describe('VillainSelectorComponent', () => {
  let component: VillainSelectorComponent;
  let fixture: ComponentFixture<VillainSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VillainSelectorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VillainSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
