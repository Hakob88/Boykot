using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boycott.WP8
{
    class BoycottBarcodes
    {
        private const string BARCODE_KEY = "code";
        public List<Int64> Barcodes { get { return barcodes; } }
        public BoycottBarcodes()
        {
            //TODO: request to the url or something
            barcodes = new List<Int64>();
            string json = "[{\"code\" : \"123456\"}, {\"code\" : \"0123456789012\"}, {\"code\" : \"90123456789\"}, {\"code\" : \"671860013624\"}, {\"code\" : \"4200006200\"}, {\"code\" : \"65833254\"}]";
            initBarcodeListFromJSON(json);
        }

        private void initBarcodeListFromJSON(string json)
        {
            barcodes.Clear();
            //JObject root = JObject.Parse();
            JArray jsonArrray = JArray.Parse(json);
            foreach (var item in jsonArrray)
            {
                Int64 code = Int64.Parse((string)item[BARCODE_KEY]);
                barcodes.Add(code);
            }
        }
        public bool isBoycottBarcode(Int64 barcode)
        {
            if (barcodes.Contains(barcode))
            {
                return true;
            }
            return false;
        }

        public bool isBoycottBarcode(string barcode)
        {
            return isBoycottBarcode(Int64.Parse(barcode));
        }
        private List<Int64> barcodes;
    }
}
