$wc = New-Object system.Net.WebClient;
$result = $wc.downloadString("https://ecosaver-compo-preprod.azurewebsites.net/schedule?key=uDG81TICusSd")