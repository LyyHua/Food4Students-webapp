import type { Metadata } from "next";
import "./globals.css";
import { getCurrentUser } from "./actions/authActions";
import ToasterProvider from "./providers/ToasterProvider";
import SignalRProvider from "./providers/SignalRProvider";
import NavBar from "./nav/NavBar";
import StickyCartBar from "./components/StickyCartBar";

export const metadata: Metadata = {
  title: "Food4Students",
  description: "Order your favorite food from your local restaurants with affordable prices",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const user = await getCurrentUser()
  const notifyUrl = process.env.NOTIFY_URL
  return (
    <html lang="en">
      <body>
        <ToasterProvider />
        <NavBar/>
        <main className="mx-auto px-14 pt-10">
          <SignalRProvider user={user} notifyUrl={notifyUrl!}>
            {children}
          </SignalRProvider>
        </main>
        <StickyCartBar />
      </body>
    </html>
  );
}
