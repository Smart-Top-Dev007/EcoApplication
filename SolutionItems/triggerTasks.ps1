param (
    [string]$url
 )
$wc = New-Object system.Net.WebClient;
$result = $wc.downloadString($url)