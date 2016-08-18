
# todo: Remove hardcoded values

$urlsToTest =
@(
'http://localhost:65461/test/CanGetLocations', `
'http://localhost:65461/test/CanCreateClient', `
'http://localhost:65461/test/CanSubmitTestAsync' `
)

$urlsToTest |  `
 foreach-object { 
    $request = [System.Net.WebRequest]::Create($_.ToString())

    Try
    {
        $response = $request.GetResponse() 
        $status = [int]$response.StatusCode

        If ($status -eq 200) 
        { 
            Write-Host $_.ToString()  " OK!" 
        }
        Else 
        {
            Write-Host $_.ToString()  " Failed!" 
        }
    }
    Catch
    {
        Write-Host $_.ToString()  " Failed!" 
    }
 }
