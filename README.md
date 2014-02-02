EventLogCatcher

Record events on multiple File Formats Understanding SOLID

===================================EventLogCatcherManager======================

EventLogCatcher at the beginning was Some lines of Code throw them by an ex- college Fernando Flores few years ago,

Now i want to accomplish this Lines into a more Simple Tool  Following 
SOLID principles, and to extend this to more File Types and Templates.

hopping the community will help making this SOLID principles more understandable for me.

Logcatcher was build to store any fired event/alarm/information/errors  over any place of the code or event  better use under the cacth sentence.


It has so far Following Estructure 

folder estructure 

/CORE 

   Logdirector : will be the only Class to be instantiate at the moment to call save the ”suceso” LOG, 
                     and according to the Implementation of any MediaAbstract class received over the constructor, 
                     then it will creates the object and Appropriate template to write.

   SucesoAbstract: abstract classestablish 2 specific methods to be override and a main expoused method "Execute" to execute the all recording process.
                   this method only calls the "Start()" method of any MediaAbstract chosed.
  
   MediaAbstract:  this will be the base clase for any MadiaType File, through the injected implementation of the ITemplate over Constructor
                   and Containst teh actual Implementation for the "Start()" method.

   ITemplate : public interface wich contains the contract for any Template to implement.


   TemplateTextAbstract: anstract class wich implement the ITemplate interface overriding the methods for FileName and Location
                         , creates the Header for any Text Files and the abstract methods for concrete implementations.


   TemplateXMLAbstract: anstract class wich implement the ITemplate interface overriding the methods for FileName and Location
                         , creates the Header for any Text Files and the abstract methods for concrete implementations.


/MediaBuilders 


   MediaTextBuilder:  Inherits the MediaAbstract, Wrapping the COnstructor of the base clase,wich recibing ITemplate implementation -TemplateTextAbstract only for TEXT files.

   MediaXMLBuilder: Inherits the MediaAbstract, Wrapping the COnstructor of the base clase,wich recibing ITemplate implementation -TemplateXMLAbstract only for XML files.


/Sucesos 

      SucesoEventBuilder :Concrete Implementation of SucesoAbstract Coordinates the Instance Implementation for MediaAbstract depending on LogConfig.txt
                          it will creates the MediaAbstract  and the appropiate ITemplate implmentation.



/Templates 

     TemplateTextEvent:     COncrete object implementation of TemplateTextAbstract, This will create and fill the apropiate format for Events in a Text File. 

     TemplateTextInterface: COncrete object implementation of TemplateTextAbstract, This will create and fill the apropiate format for Interfaces in a Text File.

     TemplateXMLEvent     : COncrete object implementation of TemplateXMLAbstract , This will create and fill the apropiate format for Events in a XML File.


/Entities: 

   Contains all DTO used in the Library




the Folder locations and configurations are in the LogConfig.txt File



Currently I have used this project for saving Events and The log of the interfaces on small projects.


Also in the Solution it’s included a Test Project in case you decide to start debuging the Program.


Thanks in advance and please feel free to Help out throughout my SOLID acknowledge


Archivo de especificaciones sobre configuración de EventLog.
