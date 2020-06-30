namespace ImageBox
{
    public static class AppExtension
    {
        public static string bytesToHuman(this double size)
        {
            long Kb = 1 * 1024;
            long Mb = Kb * 1024;
            long Gb = Mb * 1024;
            long Tb = Gb * 1024;
            long Pb = Tb * 1024;
            long Eb = Pb * 1024;

            if (size < Kb) return (size).ToString("#.##") + " byte";
            if (size >= Kb && size < Mb) return ((double)size / Kb).ToString("#.##") + " Kb";
            if (size >= Mb && size < Gb) return ((double)size / Mb).ToString("#.##") + " Mb";
            if (size >= Gb && size < Tb) return ((double)size / Gb).ToString("#.##") + " Gb";
            if (size >= Tb && size < Pb) return ((double)size / Tb).ToString("#.##") + " Tb";
            if (size >= Pb && size < Eb) return ((double)size / Pb).ToString("#.##") + " Pb";
            if (size >= Eb) return ((double)size / Eb).ToString("#.##") + " Eb";

            return "???";
        }
    }
}
