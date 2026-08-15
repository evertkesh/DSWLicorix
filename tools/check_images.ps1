$uri='http://localhost:5000/Home/ListImages'
Invoke-WebRequest $uri -UseBasicParsing -OutFile list.json
$li = Get-Content -Raw list.json | ConvertFrom-Json
$ok=0; $miss=0
foreach($i in $li){
  $url = $i.imagenURL
  if($url.StartsWith('/')){
    $rel = Join-Path (Get-Location) ($url.TrimStart('/') -replace '/','\\')
  } else {
    $rel = Join-Path (Get-Location) ('wwwroot\\imagenes\\productos\\' + $url)
  }
  if(Test-Path $rel){
    $ok++; Write-Output ("OK`t{0}`t{1}`t{2}" -f $i.idProducto,$url,(Resolve-Path $rel))
  } else {
    $miss++; Write-Output ("MISSING`t{0}`t{1}`t{2}" -f $i.idProducto,$url,$rel)
  }
}
Write-Output ("SUMMARY`tOK={0}`tMISSING={1}" -f $ok,$miss)
