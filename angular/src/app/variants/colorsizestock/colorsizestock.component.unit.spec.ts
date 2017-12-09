import { ColorSizeStockComponent } from './colorsizestock.component';
import { ToastMsgService } from '../../../services/toastmsg.service';

describe('Colorsizestock', () => {
    let component: ColorSizeStockComponent;
    let toastMsg: ToastMsgService;

    beforeEach(() => {
        toastMsg = new ToastMsgService(null);
        component = new ColorSizeStockComponent(toastMsg);
        component.quantity = 1;
    });

    it('Qty should be increased by one when + button is clicked', () => {
        component.remainingQty = '5';
        component.addOne();
        expect(component.quantity).toBe(2);
    });
    
    it('Qty should not be increased if quantity is same as remaining quantity', () => {
        component.remainingQty = '1';
        spyOn(toastMsg, 'popToast').and.returnValue(null);
        component.addOne();
        expect(component.quantity).toBe(1);
    });

    it('Qty should be reduced by one when - button is clicked', () => {
        component.quantity = 3;       
        component.reduceOne();
        expect(component.quantity).toBe(2);
    });

    it('Qty should not be reduced by one when - button is clicked while quantity is 1', () => {                      
        component.reduceOne();
        expect(component.quantity).toBe(1);
    });
})