---
title: "FAQ"
weight: 80
anchor: "faq"
---

Here are some of the Frequently Asked Questions about Dust Mother.

### Why can't I edit `<x>` value?

Due to the fragile nature of the game save files and just how many properties are in there, editing support needs to be included on a per-property basis. Out of the gate, we've included support for editing the most common properties you might want to change, but we will be adding more editable properties in time.

### Can I use this on other systems?

The full Dust Mother app is currently Windows-only and available from the Microsoft Store. There is also an in-progress command-line version of Dust Mother but that is not ready for general use yet. Stay tuned!

### Can this corrupt my save file?

Realistically? Yes. Pretty much any save editing carries some risk of breaking your save file in new and exciting ways. Dust Mother will not modify (or even write to) your save file if you are only viewing your statistics and save data. Once you modify a value and click Save, your main save file will be _overwritten_. We recommend always keeping up a backup of your save file and you can even use the "Backups" tab of Dust Mother to create new backups of your save files.

### Why does Dust Mother require so many permissions?

This is out of my hands **for now**. The runtime that this app uses (WinUI 3 Desktop for those curious) only officially supports running in "full trust" mode which means it runs unrestricted just like any other app on your PC. In the Microsoft Store, that is indicated by the ominous "Access all your files, ..." message. Microsoft have indicated that the framework will be updated to run with limited permissions in future, and I'll update the app to use that model as soon as its ready.