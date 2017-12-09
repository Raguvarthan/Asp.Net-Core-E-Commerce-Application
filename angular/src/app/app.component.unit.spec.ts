import { TestBed, ComponentFixture } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('Unit test cases for App Component', ()=> {

let component: AppComponent;
let fixture: ComponentFixture<AppComponent>;

beforeEach(()=>{
    TestBed.configureTestingModule({
        declarations: [AppComponent]
    });
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
})
})