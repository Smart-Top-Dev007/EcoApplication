$wc = New-Object system.Net.WebClient;
$result = $wc.downloadString("https://ecosaver-compo.azurewebsites.net/schedule?key=uDG81TICusSd")