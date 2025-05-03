### Dev Notes
My developer notes is a journal that reflects both the process and internal reasoning of why things are the way they are in this project.

# 4/26/2025 - `console.write(Hello World! 💻);`
🌙
On 4/26/2025 I had an epiphany, I realized that most Windows 10/11 End Users have no understanding of the Windows Firewall

This is something, I'm very sad about. There's so many issues that could be mitigated if only we made the process simpler to understand.

This tool aims to make the Whitlisting Process a breeze for the end user, it's total control over your firewall.

You can take your privacy back into your own hands, and this isn't something new: it's just something that many others

don't integrate purely because of the Time Intensive Task of Updating Outbound/Inbound Rules manually.

---

I told myself I needed to get started, I looked online for similar tools/utilities, and found nothing..

This strengthened my drive to plan, and envision a product that would help secure every user regardless of their technical knowledge.

But before I began designing: I had to understand what functions my utility must be responsible for.

So far: Displaying Blocked/Allowed IPs/Apps + Logs (and Archiving for Forensic Analysis)| Managing Blocked/Whitelisted IPs/Apps | Manual Whitelisting/Blocking of IPs/APPs | Automatic Application Whitelisting/Blocking | Auto Block Malicious IPs


Then @ 6 PM:
I began prototyping, and designing a mockup of what the finished product would be.
I spent probably about *2-3 hours* on the mockup before I decided to open visual studio.

I designed: 
  The Launcher, 
  Custom Dialog (Modularly), 
  and the Dashboard.

After I had everything mapped out, I begun transfering my Design Attributes into XAML manually.

---

EZ Guide for WPF Design:

Step 1: Design in Figma using Frames

Step 2: Design More

Step 3: Once finished reference your properties between XAML and your Figma Design (most of it can  be re-used)

---

*I finished the Splashscreen that initializes the application* and applies the **Firewall Changes** we need via **PowerShell**.

It's quite simple, the window is comprised of a Border, an image (CyberWatch Logo Banner), and 2 Text Labels.

I used an Animation Class (like a library) to make Animating the objects easier.

> I spent a total of 4 hours configuring the initial process

I then went to bed to make sure I could continue this wonderful project in the morning 🛏️💤

# 4/27/2025 - 🧑‍💻 !Day1

☀️ 
On 4/27/2025 it was a bit smoother going, I only had to work on the User Experience and tinkered with the animations present in the Splashscreen.

I Updated the Splashscreen's init logic to not repeat it's steps unless a System Variable was written to signifying the Setup Process didn't need to happen again.

After that: I implemented my Figma Design for the Dashboard into my  `Dashboard.xaml`, I began translating my design from Designer to Editor. It took only 4 Hours.

Around Noon: I spent some time working on creating custom Styles for the components in my WPF Dashboard.

Such as: { Buttons (BG:Black;Fore:White;Outline:Red;), Labels (Fore:white;Fore[onhover]:red;) and some other changes and styling as needed. }

I had completed most of the Front End for the Dashboard, and that made me very excited!

I just needed to create widgets for the Dashboard (I ended up messing up BIG TIME, I didn't design these widgets on a stackpanel/grid, and as a result hiding these elements has to be done individually programmatically until I redesign later.)

After the dashboard was finally finished, I was very excited.

(First Menu out of 3[*+?])

🛌💤 - 2am

# 4/28/2025 - ⚡ Tragedy Strikes ⛈️

☀️😎

After my morning routine, I continued my development process.

I finished the first Page of the Dashboard, and thought I would include an animation for the Counting Widgets.

Before I wanted to Animate anything, I wanted to obtain data persistence.

I attempted to sloppily integrate JSON and Failed COUNTLESS TIMES.

I wanted to give up, so badly. But it wasn't an answer because people SERIOUSLY NEED THIS.

I reminded myself why Persistence is so important. "It's all about fighting and continuing to do something in the face of adversity even where most would stop".

But as if that wasn't bad enough, from experimenting with different nuGet packages, I somehow corrupted my `.csproj` file.

I decided I would leave it for the night, and return to it tomorrow for a Total Transplant.

🙁

# 4/29/2025 - ⚕️ .NET Project Transplant 💔

😩 I woke up and decided I would create a new solution, and migrate the Entirety of the Project over, I had completely Corrupted the `.csproj` file.

I then spent the next 4-6 hours translating everything over and ensuring everything wasn't broken (we were just working on the Config before it broke)

I was eventually able to  get it into stable condition without any errors, and no exceptions on runtime! (I was estatic!)

I spent 1-2 hours researching data persistence in C# using Packages like `Microsoft.Extensions.Json` and even `Config.NET`.

I couldn't find a way that I could integrate into my UI Thread effectively, apparently in C# it's alot harder to asynchronously read/write in different scenarios. (I was also using .NET 9)

# 4/30/2025 - 📉 Hospitalized by Data Persistence

After spending hours running circles attempting to establish Data Persistence, I stressed myself out to the point of triggering my own anxiety.

Kind of funny right? I have a strong vision for this project, and I really want this to be something that others will find usefull, I've had to mitigate-
several problems throughout the development process (e.g Corrupting my `.csproj`, and other anomalies.)

The combination of my turbulent home life and my failed attempts to establish this, led to a prompt ER visit- where I discovered that I was experiencing a "Panic Attack" due to un-managed stress. (I have an auto-immune disease that's labeled as Chronic Asthma, I've lived with this condition for a while: anything that reminds me of that sensation isn't very comforting..)

I pushed myself where I should've given myself a break. I've been going at this non-stop for the past couple days.

### ⚠️ IMPORTANT ⚠️

*Mental Health* is _VERY IMPORTANT_ and burn out is unfortunately something that plagues many developers.

_Remember_: *Persistence is the act of persevering where others would give up.*

It's alright to take a break, and reflect on what you've been doing. Sometimes it's highly beneficial.

This insight came from my extensive experience in Design. I've learned that if you feel like something is missing, it's best to take a break and look at things from another perspective.

# 5/1/2025 - 200 OK
☀️😎
I took a well deserved break, and discovered something that reignited and fueled the very passion I had initially burned out from extensive failure.

Have you ever heard of [Outpost Firewall Pro](https://en.wikipedia.org/wiki/Outpost_Firewall_Pro)? I hadn't until I was in a VC with some peers and this was subsequently brought to my attention!

It feels like I'm not only supporting the Open-Sourced Community, but also filling the void that [Outpost Firewall Pro](https://en.wikipedia.org/wiki/Outpost_Firewall_Pro) when it was decommissioned.

> Not to mention this application doesn't require a subscription. No Licenses to Bypass and No Reveng Needed for Total Control!

I really hope this project grows to help others, regardless of their technical experience.

# 5/2/2025 - Redesign.

Taking inspiration from [Outpost Firewall Pro](https://en.wikipedia.org/wiki/Outpost_Firewall_Pro), I've decided to inspire the dashboard from their old product, it's almost the same; but with my twists.

### ⏱️ From 10:30 AM - 02:30 PM
I worked on prototyping for redesigning and fixing multiple bugs, including but not limited to: Custom Dialog Exceptions (temporarily replaced with MessageBox.Show), and Other issues relating to the redesign.

### ⏱️ From 04:30 PM - 08:30 PM
I redesigned `Dashboard.xaml` (properly this time with stackpanels for organization)  and implemented the new design aesthetic for the Dashboard, matching the bootstrapper/loader that preceeds it.

### 📝 Remember: If you do something right the first time, you won't have to do it again. (Do your research, take notes, and attempt to prepare as much as possible. It's a good practice to be prepared.)

> After that, I managed to complete the First Page of the Dashboard: "My Status" (not Dynamically Linked or anything yet..)

### ⏱️ From 09:45 PM - 11:30 PM
Working on the Dev-Docs Write-Up, cleaning the clutter from my R/D session, and getting things prepared for a busy day tommorrow!

### 11:30 🛌💤 Bed Time
Looking forward to Tomorrow! I'm committed to keeping my thought process Open-Sourced. I believe it will not only serve testimony for my abilities but also empower those seeking to take on something similar.

# 5/3/2025 - TBD (TOMORROW)
