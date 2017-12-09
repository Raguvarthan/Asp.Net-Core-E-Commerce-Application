import { Component, OnInit, ViewChild } from '@angular/core';
import { AppState } from '../app.service';
import { Title } from './title';
import { CartService } from '@services/cart.service';
import { ApiService } from '@services/api.service';
import { CategoryComponent } from './category';

@Component({
  /**
   * The selector is what angular internally uses
   * for `document.querySelectorAll(selector)` in our index.html
   * where, in this case, selector is the string 'home'.
   */
  selector: 'home',  // <home></home>
  /**
   * We need to tell Angular's Dependency Injection which providers are in our app.
   */
  providers: [
    Title
  ],
  /**
   * Our list of styles in our component. We may add more to compose many styles together.
   */
  styleUrls: ['./home.component.scss'],
  /**
   * Every Angular template is first compiled by the browser before Angular runs it's compiler.
   */
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  /**
   * Set our default values
   */
  public localState = { value: '' };

  public imageSources: string[] = [
    '../assets/img/sliderhome/desktop/1200-X-450_B_01.jpg',
    '../assets/img/sliderhome/desktop/1200-X-450_B_02.jpg',
    '../assets/img/sliderhome/desktop/1200-X-450_B_03.jpg',
    '../assets/img/sliderhome/desktop/1200-X-450_B_04.jpg',
    '../assets/img/sliderhome/desktop/1200-X-450_B_05.jpg',
    '../assets/img/sliderhome/desktop/1200-X-450_B_06.jpg',
  ];
  public mobileImageSources: string[] = [
    '../assets/img/sliderhome/mobile/600X600_1.jpg',
    '../assets/img/sliderhome/mobile/600X600_2.jpg',
    '../assets/img/sliderhome/mobile/600X600_3.jpg',
    '../assets/img/sliderhome/mobile/600X600_4.jpg'
  ];
  public swipeconfig: any = {
    prevButton: '.swiper-button-prev',
    nextButton: '.swiper-button-next',
    loop: true,
    autoplay: 3000,
    autoplayDisableOnInteraction: false,
    autoplayStopOnLast: false,
    loopedSlides: 20,
    loopAdditionalSlides: 20
  };
  public mobileconfig: any = {
    prevButton: '.swiper-button-prev',
    nextButton: '.swiper-button-next',
    loop: true
  };

  public category: any[] = [];
  public imgStyle: any = 'block';

  /**
   * TypeScript public modifiers
   */
  constructor(public appState: AppState, public title: Title,
              private cartServ: CartService, private apiService: ApiService) {
  }

  public ngOnInit() {
    this.GetCategories();
  }

  public GetCategories() {
    this.apiService.get('category', { useAuth: false }).then(
      (response: any) => {
        this.category = response.data;
      })
      .catch((error: any) => {
        console.log(error);
      });
  }
  public onCloseImg() {
    this.imgStyle = 'none';
  }
}
