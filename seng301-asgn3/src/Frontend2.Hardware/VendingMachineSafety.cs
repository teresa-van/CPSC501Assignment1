using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend2.Hardware
{
    public class VendingMachineSafety
    {
        public bool SafetyOn { get; protected set; }
        public CoinSlot CoinSlot { get; protected set; }
        public DeliveryChute DeliveryChute { get; protected set; }
        public IndicatorLight ExactChangeLight { get; protected set; }
        public IndicatorLight OutOfOrderLight { get; protected set; }
        public PopCanRack[] PopCanRacks { get; protected set; }
        public CoinRack[] CoinRacks { get; protected set; }

        public VendingMachineSafety(bool safetyOn, CoinSlot coinSlot, DeliveryChute deliveryChute, 
            IndicatorLight exactChangeLight, IndicatorLight outOfOrderLight, PopCanRack[] popCanRacks, CoinRack[] coinRacks)
        {
            this.SafetyOn = safetyOn;
            this.CoinSlot = coinSlot;
            this.DeliveryChute = deliveryChute;
            this.ExactChangeLight = exactChangeLight;
            this.OutOfOrderLight = outOfOrderLight;
            this.PopCanRacks = popCanRacks;
            this.CoinRacks = coinRacks;
        }


        /**
* Disables all the components of the hardware that involve physical
* movements. Activates the out of order light.
*/
        public void EnableSafety()
        {
            this.SafetyOn = true;
            this.CoinSlot.Disable();
            this.DeliveryChute.Disable();

            foreach (var popCanRack in this.PopCanRacks)
            {
                popCanRack.Disable();
            }

            foreach (var coinRack in this.CoinRacks)
            {
                coinRack.Disable();
            }

            this.OutOfOrderLight.Activate();
        }

        /**
        * Enables all the components of the hardware that involve physical
        * movements. Deactivates the out of order light.
        */
        public void DisableSafety()
        {
            this.SafetyOn = false;
            this.CoinSlot.Enable();
            this.DeliveryChute.Enable();

            foreach (var popCanRack in this.PopCanRacks)
            {
                popCanRack.Enable();
            }
            foreach (var coinRack in this.CoinRacks)
            {
                coinRack.Enable();
            }

            this.OutOfOrderLight.Deactivate();
        }
    }
}
