import {
  Component
} from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'footer',  // <footer></footer>

  styleUrls: [ './footer.component.css' ],

  templateUrl: './footer.component.html'
})
export class FooterComponent {

public onSubscribe(form: NgForm) {
console.log(form.value);
}
}
