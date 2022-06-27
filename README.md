# INTRODUCTION ABOUT SUBJECT
* *Subject:* Introduction To Software Engineering
* *Class:* SE104.M21.PMCL
* *Lecture:* sc.Nguyen Thi Thanh Truc
# OTDStore
<h2>Function summary</h2>
-Customer
+ Log in, Sign up
+ View products, buy and order
+ View basic information of the account
+ View your account's order information

-Admin:
+ Order
+ Account
+ List of customers, admin
+ Control product information
+ Print invoice

# INSTALLING 
<br>To install it on the user's machine, first, the user downloads the database at the link below:</br>
<br> **https://drive.google.com/drive/folders/1QmcPsLa4IKVFtwelo83XeGADOxa-Z7n7** </br>
<br>After the user downloads and decompresses the file, it is necessary to move the files received when decompressing to the following path:</br>
<br> **C:\Program Files\Microsoft SQL Server\MSSQLXX.MSSQLSERVER\MSSQL\DATA** </br>
<br> Then proceed to open MSSQL Sever application, go to **Database** -> **Attach** -> Select file **OTDStore** and install </br>
<br><h3>Next step, Open Visual Studio, in the **Get started** section on the right, select **Clone a repository**</h3></br>
<br>At the line **Repository location**, the user assigns the path of this git to</br>
<br>**https://github.com/thuysnga/OTDStore**</br>
<br>After completing the assignment of the path to the git file, in the path section, the user creates a new directory to clone git on the machine.</br>
<br>Complete the above step, select **Clone**</br>
<br><h3>When the machine has finished cloning, at the **Solution** table obtained later, the user selects the file **OTD.sln** </h3></br>
![image](https://user-images.githubusercontent.com/68022476/175953423-d6159163-5a9e-42d9-bdb0-053347b42526.png)
<br>After opening the **OTD.sln** file, **Domain** file, expand the **OTDStore.Data** tab and select the appsetting.json file</br>
![image](https://user-images.githubusercontent.com/68022476/175953484-bebbf205-bfaf-470b-b355-2ed39b63fde4.png)
<br>Change **sever** according to the MSSQL Sever application on your machine</br>
![image](https://user-images.githubusercontent.com/68022476/175953655-023f7b69-ad35-486d-8d07-f7c57f936154.png)
<br>Do the same with two files:</br>
<br>**OTDStore.AdminApp/appsettings.Development.json**</br>
<br>And</br>
<br>**OTDStore.BackendApi/appsettings.Development.json**</br>
<br><h3>Next, right click on the **Solution** bar to open **Property**</h3></br>
<br>Select **Multiple startup projects**</br>
<br>**Start** the following **projects**:</br>
<br>**OTDStore.WebApp</br>
<br>OTDStore.AdminApp</br>
<br>OTDStore.BackendApi** (and put this project at the top of the list)</br>
![image](https://user-images.githubusercontent.com/68022476/175953728-bdaa3c02-0a94-486f-942a-06d04e5cb7fa.png)
<br>And **OK**</br>
<br>Before **RUN** project, adjust **Startup Projects** method set with corresponding **Debug Properties**</br>
![image](https://user-images.githubusercontent.com/68022476/175953793-06967976-7e35-4c73-9fb8-7298c18c4d08.png)
![image](https://user-images.githubusercontent.com/68022476/175953812-d0649615-c21e-4409-9049-23e59c394098.png)
![image](https://user-images.githubusercontent.com/68022476/175953834-a49b026e-5979-440d-b7aa-6e370095fdbd.png)
