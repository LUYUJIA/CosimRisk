# CosimRisk

### abstract:
  Complex projects usually have a long construction period, limited resources and many uncertain factors. Project risk assessment is the key to success for projects, and schedule risk analysis is an indispensable part of it.
  
### object and solved problem:  
  The system is to evaluate schedule risk of a project by modeling the project into an weighted oriented graph and figure out critical path (at least how many days should be taken in project) by topological sort. Moreover, In order to sever more users, this system is designed according to MVC architecture, so we used oracle 10g as database and used HTML and Ext/Js to create front-end.
  
  The system parses the XML file of projectand transform it into weighted oriented graph constructed by tasks (the rectangles in the Fig.1). In order to find out critical path, The system topologically sorts them from top to end and calculates expectation of schedule days in the  same time. Then again topologically sort them from end to top, comparing whther the expectation of schedule days is thesame as top-end sequence. the tasks have same value mean that them are fixed and can not be shifted. Therefore, We find out the critical path.


<img src="images/fig1.jpg" width="700px"/>

Fig.1 Project’s tasks oriented graph(Chinese interface version)

### architecture
