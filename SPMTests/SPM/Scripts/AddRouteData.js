function AddData() {
            var regionDdl = $('#Regions')[0];
            var pins = [];
            var regionFilter = 0; if (regionDdl.options[regionDdl.selectedIndex].value != '') 
                {regionFilter = regionDdl.options[regionDdl.selectedIndex].value;}
            var reportType = $('#MetricTypes')[0]; 
            var reportTypeFilter = 0; if (reportType.options[reportType.selectedIndex].value != '')
return pins;}