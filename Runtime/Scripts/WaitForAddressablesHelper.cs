using UnityEngine;

namespace SUP.AddressablesHelper {
    public class WaitForAddressablesHelper : CustomYieldInstruction {
        public override bool keepWaiting => _isLoading;
        private bool _isLoading;

        internal WaitForAddressablesHelper() {
            _isLoading = true;
        }

        internal void StopWaiting() {
            _isLoading = true;
        }
    }
}