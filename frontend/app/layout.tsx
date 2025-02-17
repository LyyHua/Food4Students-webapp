import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import { getCurrentUser } from "./actions/authActions";
import ToasterProvider from "./providers/ToasterProvider";
import SignalRProvider from "./providers/SignalRProvider";
import NavBar from "./nav/NavBar";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

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
      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased`}
      >
        <ToasterProvider />
        <NavBar/>
        <main className="container mx-auto px-5 pt-10">
          <SignalRProvider user={user} notifyUrl={notifyUrl!}>
            {children}
          </SignalRProvider>
        </main>
      </body>
    </html>
  );
}
