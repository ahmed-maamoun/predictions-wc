import { NextRequest, NextResponse } from 'next/server';
import os from 'os';

function getServerIp() {
  const interfaces = os.networkInterfaces();
  for (const name of Object.keys(interfaces)) {
    for (const iface of interfaces[name] || []) {
      if (iface.family === 'IPv4' && !iface.internal) {
        return iface.address;
      }
    }
  }
  return null;
}

export async function GET(req: NextRequest) {
  const ip = getServerIp();
  return NextResponse.json({ ip });
} 